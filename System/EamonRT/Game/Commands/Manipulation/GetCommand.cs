﻿
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GetCommand : Command, IGetCommand
	{
		/// <summary></summary>
		protected virtual bool OmitWeightCheck { get; set; }

		public virtual bool GetAll { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> ArtifactList { get; set; }

		public virtual void ProcessAction(Action action, ref bool nlFlag)
		{
			Debug.Assert(action != null);

			if (nlFlag)
			{
				gOut.WriteLine();

				nlFlag = false;
			}

			action();

			if (!PreserveNextState && NextState != null)
			{
				NextState = null;
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			if (ac.Type == ArtifactType.DisguisedMonster)
			{
				ProcessAction(() => gEngine.RevealDisguisedMonster(ActorRoom, artifact), ref nlFlag);
			}
			else if (artifact.Weight > 900)
			{
				ProcessAction(() => PrintDontBeAbsurd(), ref nlFlag);
			}
			else if (artifact.IsUnmovable01())
			{
				ProcessAction(() => PrintCantVerbThat(artifact), ref nlFlag);
			}
			else
			{
				var count = 0L;

				var weight = artifact.Weight;

				if (artifact.GeneralContainer != null)
				{
					rc = artifact.GetContainerInfo(ref count, ref weight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = artifact.GetContainerInfo(ref count, ref weight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				var charWeight = 0L;

				rc = ActorMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (ac.Type == ArtifactType.DeadBody && ac.Field1 != 1)
				{
					ProcessAction(() => PrintBestLeftAlone(artifact), ref nlFlag);
				}
				else if (!OmitWeightCheck && (weight + charWeight > ActorMonster.GetWeightCarryableGronds()))
				{
					ProcessAction(() => PrintTooHeavy(artifact), ref nlFlag);
				}
				else if (ac.Type == ArtifactType.BoundMonster)
				{
					ProcessAction(() => PrintMustBeFreed(artifact), ref nlFlag);
				}
				else
				{
					var monster = gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -artifact.Uid - 1 && m != ActorMonster).FirstOrDefault();

					if (monster != null)
					{
						ProcessAction(() => PrintObjBelongsToActor(artifact, monster), ref nlFlag);
					}
					else
					{
						var isCarriedByContainer = artifact.IsCarriedByContainer();

						artifact.SetCarriedByCharacter();

						if (NextState is IRequestCommand)
						{
							PrintReceived(artifact);
						}
						else if (NextState is IRemoveCommand || isCarriedByContainer)
						{
							PrintRetrieved(artifact);
						}
						else
						{
							PrintTaken(artifact);
						}

						nlFlag = true;
					}
				}
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(GetAll || DobjArtifact != null);

			if (GetAll)
			{
				// screen out all weapons in the room which have monsters present with affinities to those weapons

				ArtifactList = ActorRoom.GetTakeableList().Where(a => gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -a.Uid - 1 && m != ActorMonster).FirstOrDefault() == null).ToList();
			}
			else
			{
				ArtifactList = new List<IArtifact>() { DobjArtifact };
			}

			if (ArtifactList.Count > 0)
			{
				var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

				var nlFlag = false;

				IArtifact wpnArtifact = null;

				foreach (var artifact in ArtifactList)
				{
					var ac = artifact.GetArtifactCategory(artTypes, false);

					if (ac == null)
					{
						ac = artifact.GetCategories(0);
					}

					Debug.Assert(ac != null);

					OmitWeightCheck = artifact.IsCarriedByCharacter(true);

					ProcessArtifact(artifact, ac, ref nlFlag);

					if (artifact.IsCarriedByCharacter())
					{
						// when a weapon is picked up all monster affinities to that weapon are broken

						var fumbleMonsters = gEngine.GetMonsterList(m => m.Weapon == -artifact.Uid - 1 && m != ActorMonster);

						foreach (var monster in fumbleMonsters)
						{
							monster.Weapon = -1;
						}

						var ac01 = artifact.GeneralWeapon;

						if (artifact.IsReadyableByCharacter() && (wpnArtifact == null || gEngine.WeaponPowerCompare(artifact, wpnArtifact) > 0) && (!GetAll || ArtifactList.Count == 1 || gGameState.Sh < 1 || ac01.Field5 < 2))
						{
							wpnArtifact = artifact;
						}
					}
				}

				if (nlFlag)
				{
					gOut.WriteLine();

					nlFlag = false;
				}

				if (ActorRoom.IsLit())
				{
					if (!gEngine.AutoDisplayUnseenArtifactDescs && !GetAll && DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.Seen)
					{
						Globals.Buf.Clear();

						var rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Write("{0}", Globals.Buf);

						DobjArtifact.Seen = true;
					}
				}

				if (ActorMonster.Weapon <= 0 && wpnArtifact != null && NextState == null)
				{
					var command = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(command);

					command.Dobj = wpnArtifact;

					NextState = command;
				}
			}
			else
			{
				gOut.Print("There's nothing for you to get.");

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
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

			Debug.Assert(DobjArtifact != null);

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

							gOut.Print("{0} picks up {1}.", monsterName, DobjArtifact.GetTheName());
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

		public GetCommand()
		{
			SortOrder = 160;

			Name = "GetCommand";

			Verb = "get";

			Type = CommandType.Manipulation;
		}
	}
}
