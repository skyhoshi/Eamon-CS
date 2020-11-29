﻿
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : Command, IRemoveCommand
	{
		public long _dobjArtifactCount;

		public long _dobjArtifactWeight;

		public long _actorMonsterInventoryWeight;

		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ArmorArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ShieldArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ArmorArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact ShieldArtifact { get; set; }

		/// <summary></summary>
		public virtual string MonsterName { get; set; }

		/// <summary></summary>
		public virtual long DobjArtifactCount
		{
			get
			{
				return _dobjArtifactCount;
			}

			set
			{
				_dobjArtifactCount = value;
			}
		}

		/// <summary></summary>
		public virtual long DobjArtifactWeight
		{
			get
			{
				return _dobjArtifactWeight;
			}

			set
			{
				_dobjArtifactWeight = value;
			}
		}

		/// <summary></summary>
		public virtual long ActorMonsterInventoryWeight
		{
			get
			{
				return _actorMonsterInventoryWeight;
			}

			set
			{
				_actorMonsterInventoryWeight = value;
			}
		}

		/// <summary></summary>
		public virtual bool OmitWeightCheck { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			if (IobjArtifact != null)
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IRemoveCommand>(DobjArtifact, false);

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (ActorMonster.Weapon <= 0 && DobjArtifact.IsReadyableByCharacter() && NextState == null)
				{
					NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand, false);
				}
			}
			else
			{
				ArmorArtifact = gADB[gGameState.Ar];

				ShieldArtifact = gADB[gGameState.Sh];

				ArmorArtifactAc = ArmorArtifact != null ? ArmorArtifact.Wearable : null;

				ShieldArtifactAc = ShieldArtifact != null ? ShieldArtifact.Wearable : null;

				if (DobjArtifact.Uid == gGameState.Sh)
				{
					ActorMonster.Armor = ArmorArtifactAc != null ? (ArmorArtifactAc.Field1 / 2) + ((ArmorArtifactAc.Field1 / 2) >= 3 ? 2 : 0) : 0;

					gGameState.Sh = 0;
				}

				if (DobjArtifact.Uid == gGameState.Ar)
				{
					ActorMonster.Armor = ShieldArtifactAc != null ? ShieldArtifactAc.Field1 : 0;

					gGameState.Ar = 0;
				}

				DobjArtifact.SetCarriedByCharacter();

				PrintRemoved(DobjArtifact);

				PlayerProcessEvents(EventType.AfterWornArtifactRemove);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && IobjArtifact != null && Prep != null && Enum.IsDefined(typeof(ContainerType), Prep.ContainerType));

			Debug.Assert(DobjArtifact.IsCarriedByContainer(IobjArtifact) && DobjArtifact.GetCarriedByContainerContainerType() == Prep.ContainerType);

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				DobjArtAc = DobjArtifact.GetCategories(0);
			}

			if (DobjArtAc != null && DobjArtAc.Type != ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (DobjArtAc.Type != ArtifactType.DeadBody || DobjArtAc.Field1 == 1) && DobjArtAc.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = DobjArtifact.IsCarriedByMonster(ActorMonster, true);

				DobjArtifactCount = 0;

				DobjArtifactWeight = DobjArtifact.Weight;

				if (DobjArtifact.GeneralContainer != null)
				{
					rc = DobjArtifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = DobjArtifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				ActorMonsterInventoryWeight = 0;

				rc = ActorMonster.GetFullInventoryWeight(ref _actorMonsterInventoryWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || (DobjArtifactWeight <= ActorMonster.GetWeightCarryableGronds() && DobjArtifactWeight + ActorMonsterInventoryWeight <= ActorMonster.GetWeightCarryableGronds() * ActorMonster.GroupCount))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					Debug.Assert(gCharMonster != null);

					if (gCharMonster.IsInRoom(ActorRoom))
					{
						if (ActorRoom.IsLit())
						{
							MonsterName = ActorMonster.EvalPlural(ActorMonster.GetTheName(true), ActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

							gOut.Print("{0} removes {1} from {2} {3}.", MonsterName, DobjArtifact.GetArticleName(), gEngine.EvalContainerType(Prep.ContainerType, "inside", "on", "under", "behind"), OmitWeightCheck ? IobjArtifact.GetArticleName(buf: Globals.Buf01) : IobjArtifact.GetTheName(buf: Globals.Buf01));
						}
						else
						{
							MonsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

							gOut.Print("{0} picks up {1}.", MonsterName, "a weapon");
						}
					}

					// when a weapon is picked up all monster affinities to that weapon are broken

					FumbleMonsterList = gEngine.GetMonsterList(m => m.Weapon == -DobjArtifact.Uid - 1 && m != ActorMonster);

					foreach (var monster in FumbleMonsterList)
					{
						monster.Weapon = -1;
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "in", "fromin", "on", "fromon", "under", "fromunder", "behind", "frombehind", "from" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public RemoveCommand()
		{
			SortOrder = 220;

			IsIobjEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Uid = 53;

			Name = "RemoveCommand";

			Verb = "remove";

			Type = CommandType.Manipulation;
		}
	}
}
