
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GetCommand : Command, IGetCommand
	{
		public virtual IList<IArtifact> ArtifactList { get; set; }

		public virtual bool GetAll { get; set; }

		public virtual void ProcessAction(Action action, ref bool nlFlag)
		{
			Debug.Assert(action != null);

			if (nlFlag)
			{
				Globals.Out.WriteLine();

				nlFlag = false;
			}

			action();

			if (!PreserveNextState && NextState != null)
			{
				NextState = null;
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact, Classes.IArtifactCategory ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			if (ac.Type == Enums.ArtifactType.DisguisedMonster)
			{
				ProcessAction(() => Globals.Engine.RevealDisguisedMonster(artifact), ref nlFlag);
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

				var rc = artifact.GetContainerInfo(ref count, ref weight, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (ac.Type == Enums.ArtifactType.DeadBody && ac.Field1 != 1)
				{
					ProcessAction(() => PrintBestLeftAlone(artifact), ref nlFlag);
				}
				else if (Globals.GameState.Wt + weight > ActorMonster.GetWeightCarryableGronds())
				{
					ProcessAction(() => PrintTooHeavy(artifact), ref nlFlag);
				}
				else if (ac.Type == Enums.ArtifactType.BoundMonster)
				{
					ProcessAction(() => PrintMustBeFreed(artifact), ref nlFlag);
				}
				else
				{
					var monster = Globals.Engine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -artifact.Uid - 1 && m != ActorMonster).FirstOrDefault();

					if (monster != null)
					{
						ProcessAction(() => PrintObjBelongsToActor(artifact, monster), ref nlFlag);
					}
					else
					{
						Globals.GameState.Wt += weight;

						artifact.SetCarriedByCharacter();

						if (NextState is IRequestCommand)
						{
							PrintReceived(artifact);
						}
						else if (NextState is IRemoveCommand)
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

				ArtifactList = ActorRoom.GetTakeableList().Where(a => Globals.Engine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -a.Uid - 1 && m != ActorMonster).FirstOrDefault() == null).ToList();
			}
			else
			{
				ArtifactList = new List<IArtifact>() { DobjArtifact };
			}

			if (ArtifactList.Count > 0)
			{
				var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DeadBody, Enums.ArtifactType.BoundMonster, Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

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

					ProcessArtifact(artifact, ac, ref nlFlag);

					if (artifact.IsCarriedByCharacter())
					{
						// when a weapon is picked up all monster affinities to that weapon are broken

						var fumbleMonsters = Globals.Engine.GetMonsterList(m => m.Weapon == -artifact.Uid - 1 && m != ActorMonster);

						foreach (var monster in fumbleMonsters)
						{
							monster.Weapon = -1;
						}

						var ac01 = artifact.GeneralWeapon;

						if (artifact.IsReadyableByCharacter() && (wpnArtifact == null || Globals.Engine.WeaponPowerCompare(artifact, wpnArtifact) > 0) && (!GetAll || ArtifactList.Count == 1 || Globals.GameState.Sh < 1 || ac01.Field5 < 2))
						{
							wpnArtifact = artifact;
						}
					}
				}

				if (nlFlag)
				{
					Globals.Out.WriteLine();

					nlFlag = false;
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
				Globals.Out.Print("There's nothing for you to get.");

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

			var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DeadBody, Enums.ArtifactType.BoundMonster, Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

			var ac = DobjArtifact.GetArtifactCategory(artTypes, false);

			if (ac == null)
			{
				ac = DobjArtifact.GetCategories(0);
			}

			if (ac != null && (ActorRoom.IsLit() || ActorMonster.Weapon == -DobjArtifact.Uid - 1) && ac.Type != Enums.ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (ac.Type != Enums.ArtifactType.DeadBody || ac.Field1 == 1) && ac.Type != Enums.ArtifactType.BoundMonster)
			{
				var artCount = 0L;

				var artWeight = DobjArtifact.Weight;

				rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var monWeight = 0L;

				rc = ActorMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (!Globals.Engine.EnforceMonsterWeightLimits || (artWeight <= ActorMonster.GetWeightCarryableGronds() && artWeight + monWeight <= ActorMonster.GetWeightCarryableGronds() * ActorMonster.GroupCount))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					var charMonster = Globals.MDB[Globals.GameState.Cm];

					Debug.Assert(charMonster != null);

					var viewingMonsters = Globals.Engine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m != ActorMonster);

					if (viewingMonsters.Contains(charMonster))
					{
						var monsterName = ActorRoom.EvalLightLevel("An unseen offender", ActorMonster.EvalPlural(ActorMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), ActorMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01)));

						Globals.Out.Print("{0} picks up {1}.", monsterName, ActorRoom.EvalLightLevel("a weapon", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf)));
					}

					// when a weapon is picked up all monster affinities to that weapon are broken

					var fumbleMonsters = Globals.Engine.GetMonsterList(m => m.Weapon == -DobjArtifact.Uid - 1 && m != ActorMonster);

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

		public override void PlayerFinishParsing()
		{
			CommandParser.ParseName();

			if (string.Equals(CommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				GetAll = true;
			}
			else
			{
				CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsInRoom(ActorRoom),
					a => a.IsEmbeddedInRoom(ActorRoom)
				};

				CommandParser.ObjData.ArtifactNotFoundFunc = PrintCantVerbThat;

				PlayerResolveArtifact();
			}
		}

		public GetCommand()
		{
			SortOrder = 160;

			Name = "GetCommand";

			Verb = "get";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
