
// MonsterReadiesWeaponState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterReadiesWeaponState : State, IMonsterReadiesWeaponState
	{
		public virtual IList<IArtifact> ArtifactList { get; set; }

		public virtual long ArtifactListIndex { get; set; }

		public virtual long MemberNumber { get; set; }

		public virtual bool GetCommandCalled { get; set; }

		public virtual bool ReadyCommandCalled { get; set; }

		public override void Execute()
		{
			ICommand command = null;

			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			Debug.Assert(ArtifactList != null);

			if (monster.Weapon > 0 || ArtifactListIndex < 0 || ArtifactListIndex >= ArtifactList.Count)
			{
				if (monster.Weapon < 0 && monster.CombatCode == Enums.CombatCode.NaturalWeapons && monster.NwDice > 0 && monster.NwSides > 0)
				{
					monster.Weapon = 0;
				}

				if (MemberNumber > 0)
				{
					NextState = Globals.CreateInstance<IMonsterBattleState>(x =>
					{
						x.ReadyCommandCalled = true;

						x.MemberNumber = monster.Weapon != 0 || GetCommandCalled ? MemberNumber + 1 : MemberNumber;
					});
				}

				goto Cleanup;
			}

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var artifact = ArtifactList[(int)ArtifactListIndex];

			Debug.Assert(artifact != null && artifact.IsReadyableByMonster(monster));

			if (!artifact.IsCarriedByMonster(monster))
			{
				if (!GetCommandCalled)
				{
					command = Globals.CreateInstance<IGetCommand>();

					command.ActorMonster = monster;

					command.ActorRoom = room;

					command.Dobj = artifact;

					command.NextState = Globals.CreateInstance<IMonsterReadiesWeaponState>(x =>
					{
						x.ArtifactList = ArtifactList;

						x.ArtifactListIndex = ArtifactListIndex;

						x.MemberNumber = MemberNumber;

						x.GetCommandCalled = true;
					});

					NextState = command;
				}
				else
				{
					NextState = Globals.CreateInstance<IMonsterReadiesWeaponState>(x =>
					{
						x.ArtifactList = ArtifactList;

						x.ArtifactListIndex = ArtifactListIndex + 1;

						x.MemberNumber = MemberNumber;
					});
				}

				goto Cleanup;
			}

			if (!ReadyCommandCalled)
			{
				command = Globals.CreateInstance<IReadyCommand>();

				command.ActorMonster = monster;

				command.ActorRoom = room;

				command.Dobj = artifact;

				command.NextState = Globals.CreateInstance<IMonsterReadiesWeaponState>(x =>
				{
					x.ArtifactList = ArtifactList;

					x.ArtifactListIndex = ArtifactListIndex;

					x.MemberNumber = MemberNumber;

					x.GetCommandCalled = GetCommandCalled;

					x.ReadyCommandCalled = true;
				});

				NextState = command;
			}
			else
			{
				NextState = Globals.CreateInstance<IMonsterReadiesWeaponState>(x =>
				{
					x.ArtifactList = ArtifactList;

					x.ArtifactListIndex = GetCommandCalled ? ArtifactList.Count : ArtifactListIndex + 1;

					x.MemberNumber = MemberNumber;

					x.GetCommandCalled = GetCommandCalled;
				});
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterReadiesWeaponState()
		{
			Name = "MonsterReadiesWeaponState";
		}
	}
}
