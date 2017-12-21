
// GetCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public virtual bool GetAll { get; set; }

		protected virtual void ProcessAction(Action action, ref bool nlFlag)
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
				NextState.Dispose();

				NextState = null;
			}
		}

		protected virtual void ProcessArtifact(IArtifact artifact, Classes.IArtifactClass ac, ref bool nlFlag)
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

				if (ac.Type == Enums.ArtifactType.DeadBody && ac.Field5 != 1)
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

		protected override void PlayerExecute()
		{
			Debug.Assert(GetAll || DobjArtifact != null);

			var artifactList = GetAll ? ActorRoom.GetTakeableList() : new List<IArtifact>() { DobjArtifact };

			if (artifactList.Count > 0)
			{
				var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DeadBody, Enums.ArtifactType.BoundMonster, Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

				var nlFlag = false;

				IArtifact wpnArtifact = null;

				foreach (var artifact in artifactList)
				{
					var ac = artifact.GetArtifactClass(artClasses, false);

					if (ac == null)
					{
						ac = artifact.GetClasses(0);
					}

					Debug.Assert(ac != null);

					ProcessArtifact(artifact, ac, ref nlFlag);

					if (artifact.IsReadyableByCharacter() && artifact.IsCarriedByCharacter())
					{
						if (wpnArtifact == null || Globals.Engine.WeaponPowerCompare(artifact, wpnArtifact) > 0)
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

					command.DobjArtifact = wpnArtifact;

					NextState = command;
				}
			}
			else
			{
				Globals.Out.WriteLine("{0}There's nothing for you to get.", Environment.NewLine);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DeadBody, Enums.ArtifactType.BoundMonster, Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

			var ac = DobjArtifact.GetArtifactClass(artClasses, false);

			if (ac == null)
			{
				ac = DobjArtifact.GetClasses(0);
			}

			if (ac != null && ActorRoom.IsLit() && ac.Type != Enums.ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (ac.Type != Enums.ArtifactType.DeadBody || ac.Field5 == 1) && ac.Type != Enums.ArtifactType.BoundMonster)
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

					var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsInRoom(ActorRoom) && m != ActorMonster);

					if (monsters.Contains(charMonster))
					{
						var monsterName = ActorMonster.EvalPlural(ActorMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), ActorMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01));

						Globals.Out.Write("{0}{1} picks up {2}.{0}", Environment.NewLine, monsterName, DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
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

		protected override void PlayerFinishParsing()
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
