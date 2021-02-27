
// MonsterMemberActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterMemberActionState : State, IMonsterMemberActionState
	{
		/// <summary></summary>
		public Enums.Spell _spellCast;

		/// <summary></summary>
		public IGameBase _spellTarget;

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> WeaponArtifactList { get; set; }

		/// <summary></summary>
		public virtual IMonsterSpell MonsterSpell { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		/// <summary></summary>
		public virtual long WeaponArtifactListIndex { get; set; }

		/// <summary></summary>
		public virtual ContainerType WeaponContainerType { get; set; }

		/// <summary></summary>
		public virtual string ContainerPrepName { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			if (LoopMonster.ShouldReadyWeapon() && ((LoopMonster.CombatCode == CombatCode.NaturalWeapons && LoopMonster.Weapon <= 0) || ((LoopMonster.CombatCode == CombatCode.Weapons || LoopMonster.CombatCode == CombatCode.Attacks) && LoopMonster.Weapon < 0)))
			{
				WeaponArtifactList = gEngine.BuildLoopWeaponArtifactList(LoopMonster);

				if (WeaponArtifactList != null && WeaponArtifactList.Count > 0)
				{
					for (WeaponArtifactListIndex = 0; WeaponArtifactListIndex < WeaponArtifactList.Count; WeaponArtifactListIndex++)
					{
						WeaponArtifact = WeaponArtifactList[(int)WeaponArtifactListIndex];

						Debug.Assert(WeaponArtifact != null);

						if (!WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							WeaponContainerType = WeaponArtifact.GetCarriedByContainerContainerType();

							if (Enum.IsDefined(typeof(ContainerType), WeaponContainerType))
							{
								ContainerPrepName = gEngine.EvalContainerType(WeaponContainerType, "in", "on", "under", "behind");

								ActionCommand = Globals.CreateInstance<IRemoveCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;

									x.Iobj = WeaponArtifact.GetCarriedByContainer();

									x.Prep = gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(ContainerPrepName, StringComparison.OrdinalIgnoreCase));
								});
							}
							else
							{
								ActionCommand = Globals.CreateInstance<IGetCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;
								});
							}

							ActionCommand.MonsterExecute();
						}

						if (WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							ActionCommand = Globals.CreateInstance<IReadyCommand>(x =>
							{
								x.ActorMonster = LoopMonster;

								x.ActorRoom = LoopMonsterRoom;

								x.Dobj = WeaponArtifact;
							});

							ActionCommand.MonsterExecute();
						}

						if (LoopMonster.Weapon > 0)
						{
							goto Cleanup;
						}
					}
				}
			}

			if (LoopMonster.CombatCode == CombatCode.NaturalWeapons && LoopMonster.Weapon < 0)
			{
				LoopMonster.Weapon = 0;
			}

			if (LoopMonster.ShouldCastSpell(ref _spellCast, ref _spellTarget))
			{
				MonsterSpell = LoopMonster.GetMonsterSpell(_spellCast);

				if (MonsterSpell != null)
				{
					ActionCommand = null;

					switch (_spellCast)
					{
						case Spell.Blast:

							if (LoopMonster.CombatCode != CombatCode.NeverFights)
							{
								Debug.Assert(_spellTarget != null);

								ActionCommand = Globals.CreateInstance<IBlastCommand>(x =>
								{
									x.CheckAttack = true;
								});
							}

							break;

						case Spell.Heal:

							ActionCommand = Globals.CreateInstance<IHealCommand>();

							break;

						case Spell.Speed:

							Debug.Assert(_spellTarget == null);

							ActionCommand = Globals.CreateInstance<ISpeedCommand>();

							break;

						case Spell.Power:

							Debug.Assert(_spellTarget == null);

							ActionCommand = Globals.CreateInstance<IPowerCommand>();

							break;
					}

					if (ActionCommand != null)
					{
						ActionCommand.NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();

						ActionCommand.ActorMonster = LoopMonster;

						ActionCommand.ActorRoom = LoopMonsterRoom;

						ActionCommand.Dobj = _spellTarget;

						ActionCommand.MonsterExecute();

						NextState = ActionCommand.NextState;

						goto Cleanup;
					}
				}
			}

			if (LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.CheckNBTLHostility() && LoopMonster.Weapon >= 0)
			{
				NextState = Globals.CreateInstance<IMonsterAttackLoopInitializeState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterMemberActionState()
		{
			Uid = 11;

			Name = "MonsterMemberActionState";
		}
	}
}
