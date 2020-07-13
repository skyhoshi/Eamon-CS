
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : Command, IRemoveCommand
	{
		/// <summary></summary>
		protected virtual bool OmitWeightCheck { get; set; }

		/// <summary></summary>
		public const long PpeAfterWornArtifactRemove = 1;

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
				var arArtifact = gADB[gGameState.Ar];

				var shArtifact = gADB[gGameState.Sh];

				var arAc = arArtifact != null ? arArtifact.Wearable : null;

				var shAc = shArtifact != null ? shArtifact.Wearable : null;

				if (DobjArtifact.Uid == gGameState.Sh)
				{
					ActorMonster.Armor = arAc != null ? (arAc.Field1 / 2) + ((arAc.Field1 / 2) >= 3 ? 2 : 0) : 0;

					gGameState.Sh = 0;
				}

				if (DobjArtifact.Uid == gGameState.Ar)
				{
					ActorMonster.Armor = shAc != null ? shAc.Field1 : 0;

					gGameState.Ar = 0;
				}

				DobjArtifact.SetCarriedByCharacter();

				PrintRemoved(DobjArtifact);

				PlayerProcessEvents(PpeAfterWornArtifactRemove);

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

			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			var ac = DobjArtifact.GetArtifactCategory(artTypes, false);

			if (ac == null)
			{
				ac = DobjArtifact.GetCategories(0);
			}

			if (ac != null && ac.Type != ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (ac.Type != ArtifactType.DeadBody || ac.Field1 == 1) && ac.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = DobjArtifact.IsCarriedByMonster(ActorMonster, true);

				var artCount = 0L;

				var artWeight = DobjArtifact.Weight;

				if (DobjArtifact.GeneralContainer != null)
				{
					rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				var monWeight = 0L;

				rc = ActorMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || (artWeight <= ActorMonster.GetWeightCarryableGronds() && artWeight + monWeight <= ActorMonster.GetWeightCarryableGronds() * ActorMonster.GroupCount))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					var charMonster = gMDB[gGameState.Cm];

					Debug.Assert(charMonster != null);

					if (charMonster.IsInRoom(ActorRoom))
					{
						if (ActorRoom.IsLit())
						{
							var monsterName = ActorMonster.EvalPlural(ActorMonster.GetTheName(true), ActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

							gOut.Print("{0} removes {1} from {2} {3}.", monsterName, DobjArtifact.GetArticleName(), gEngine.EvalContainerType(Prep.ContainerType, "inside", "on", "under", "behind"), OmitWeightCheck ? IobjArtifact.GetArticleName(buf: Globals.Buf01) : IobjArtifact.GetTheName(buf: Globals.Buf01));
						}
						else
						{
							var monsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

							gOut.Print("{0} picks up {1}.", monsterName, "a weapon");
						}
					}

					// when a weapon is picked up all monster affinities to that weapon are broken

					var fumbleMonsters = gEngine.GetMonsterList(m => m.Weapon == -DobjArtifact.Uid - 1 && m != ActorMonster);

					foreach (var monster in fumbleMonsters)
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

			var prepNames = new string[] { "in", "fromin", "on", "fromon", "under", "fromunder", "behind", "frombehind", "from" };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
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

			Name = "RemoveCommand";

			Verb = "remove";

			Type = CommandType.Manipulation;
		}
	}
}
