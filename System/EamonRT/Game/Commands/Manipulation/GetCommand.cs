
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		public virtual bool GetAll { get; set; }

		public virtual bool OmitWeightCheck { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> ArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<string> ContainerContentsList { get; set; }

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

		public virtual void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			if (ac.Type == ArtifactType.DisguisedMonster)
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

				if (artifact.GeneralContainer != null)
				{
					rc = artifact.GetContainerInfo(ref count, ref weight, ContainerType.In, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rc = artifact.GetContainerInfo(ref count, ref weight, ContainerType.On, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				var charWeight = 0L;

				rc = ActorMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

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
					var monster = Globals.Engine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -artifact.Uid - 1 && m != ActorMonster).FirstOrDefault();

					if (monster != null)
					{
						ProcessAction(() => PrintObjBelongsToActor(artifact, monster), ref nlFlag);
					}
					else
					{
						var isCarriedByContainer = artifact.IsCarriedByContainer();

						Globals.LastArtifactLocation = artifact.Location;

						artifact.SetCarriedByCharacter();

						Globals.Engine.RevealExtendedContainerContents(ActorRoom, artifact, ContainerContentsList);

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

			ContainerContentsList = new List<string>();

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

				if (!GetAll && DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.Seen)
				{
					Globals.Buf.Clear();

					var rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.Write("{0}", Globals.Buf);

					DobjArtifact.Seen = true;
				}

				foreach (var containerContentsDesc in ContainerContentsList)
				{
					Globals.Out.Write("{0}", containerContentsDesc);
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

			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			var ac = DobjArtifact.GetArtifactCategory(artTypes, false);

			if (ac == null)
			{
				ac = DobjArtifact.GetCategories(0);
			}

			if (ac != null && ac.Type != ArtifactType.DisguisedMonster && DobjArtifact.UnderContainer == null && DobjArtifact.BehindContainer == null && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (ac.Type != ArtifactType.DeadBody || ac.Field1 == 1) && ac.Type != ArtifactType.BoundMonster)
			{
				var artCount = 0L;

				var artWeight = DobjArtifact.Weight;

				if (DobjArtifact.GeneralContainer != null)
				{
					rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.In, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.On, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				var monWeight = 0L;

				rc = ActorMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (!Globals.Engine.EnforceMonsterWeightLimits || OmitWeightCheck || (artWeight <= ActorMonster.GetWeightCarryableGronds() && artWeight + monWeight <= ActorMonster.GetWeightCarryableGronds() * ActorMonster.GroupCount))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					var charMonster = Globals.MDB[Globals.GameState.Cm];

					Debug.Assert(charMonster != null);

					if (charMonster.IsInRoom(ActorRoom))
					{
						if (ActorRoom.IsLit())
						{
							var monsterName = ActorMonster.EvalPlural(ActorMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), ActorMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01));

							Globals.Out.Print("{0} picks up {1}.", monsterName, DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
						}
						else
						{
							var monsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

							Globals.Out.Print("{0} picks up {1}.", monsterName, "a weapon");
						}
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
					a => a.IsEmbeddedInRoom(ActorRoom),
					a => a.GetCarriedByContainerContainerType() == ContainerType.On && a.GetCarriedByContainer() != null && a.GetCarriedByContainer().IsInRoom(ActorRoom)
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

			Type = CommandType.Manipulation;
		}
	}
}
