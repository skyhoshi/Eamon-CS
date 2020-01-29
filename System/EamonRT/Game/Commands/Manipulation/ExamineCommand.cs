
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : Command, IExamineCommand
	{
		/// <summary></summary>
		public const long PpeAfterArtifactFullDescPrint = 1;

		/// <summary></summary>
		public const long PpeAfterArtifactContentsPrint = 2;

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			if (gDobjArtifact != null)
			{
				var artTypes = new ArtifactType[] { ArtifactType.DoorGate, ArtifactType.DisguisedMonster, ArtifactType.Drinkable, ArtifactType.Edible, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };

				var ac = gDobjArtifact.GetArtifactCategory(artTypes, false);

				if (ac == null)
				{
					ac = gDobjArtifact.GetCategories(0);
				}

				Debug.Assert(ac != null);

				if (gDobjArtifact.IsEmbeddedInRoom(gActorRoom))
				{
					gDobjArtifact.SetInRoom(gActorRoom);
				}

				if (ac.Type == ArtifactType.DoorGate)
				{
					ac.Field4 = 0;
				}

				if (ac.Type == ArtifactType.DisguisedMonster)
				{
					gEngine.RevealDisguisedMonster(gActorRoom, gDobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				Globals.Buf.Clear();

				if (Enum.IsDefined(typeof(ContainerType), ContainerType) && !gDobjArtifact.IsWornByCharacter())
				{
					var containerArtType = gEngine.EvalContainerType(ContainerType, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer);

					var ac01 = gDobjArtifact.GetArtifactCategory(containerArtType);

					if (ac01 == null)
					{
						PrintYouSeeNothingSpecial();

						goto Cleanup;
					}

					if (ac01 == gDobjArtifact.InContainer && !ac01.IsOpen())
					{
						PrintMustFirstOpen(gDobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					var artifactList = gDobjArtifact.GetContainedList(containerType: ContainerType);
					
					var showCharOwned = !gDobjArtifact.IsCarriedByCharacter() /* && !gDobjArtifact.IsWornByCharacter() */;

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} you see ",
							Environment.NewLine,
							gEngine.EvalContainerType(ContainerType, "Inside", "On", "Under", "Behind"),
							gDobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));

						var rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, showCharOwned, StateDescDisplayCode.None, false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.SetFormat("{0}There's nothing {1} {2}",
							Environment.NewLine,
							gEngine.EvalContainerType(ContainerType, "inside", "on", "under", "behind"),
							gDobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					gOut.Write("{0}", Globals.Buf);

					PlayerProcessEvents(PpeAfterArtifactContentsPrint);

					if (GotoCleanup)
					{
						goto Cleanup;
					}
				}
				else
				{
					var rc = gDobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Write("{0}", Globals.Buf);

					gDobjArtifact.Seen = true;

					PlayerProcessEvents(PpeAfterArtifactFullDescPrint);

					if (GotoCleanup)
					{
						goto Cleanup;
					}

					if ((ac.Type == ArtifactType.Drinkable || ac.Type == ArtifactType.Edible) && ac.Field2 != Constants.InfiniteDrinkableEdible)
					{
						gOut.Print("There {0}{1}{2}{3} left.",
							ac.Field2 != 1 ? "are " : "is ",
							ac.Field2 > 0 ? gEngine.GetStringFromNumber(ac.Field2, false, Globals.Buf) : "no",
							ac.Type == ArtifactType.Drinkable ? " swallow" : " bite",
							ac.Field2 != 1 ? "s" : "");
					}

					if (((ac.Type == ArtifactType.InContainer && ac.IsOpen()) || ac.Type == ArtifactType.OnContainer || ac.Type == ArtifactType.UnderContainer || ac.Type == ArtifactType.BehindContainer) && gDobjArtifact.ShouldShowContentsWhenExamined())
					{
						var command = Globals.CreateInstance<IInventoryCommand>(x =>
						{
							x.AllowExtendedContainers = true;
						});

						CopyCommandData(command);

						NextState = command;

						goto Cleanup;
					}
				}
			}
			else
			{
				Globals.Buf.Clear();

				var rc = gDobjMonster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Write("{0}", Globals.Buf);

				gDobjMonster.Seen = true;

				if (gDobjMonster.Friendliness == Friendliness.Friend && gDobjMonster.ShouldShowContentsWhenExamined())
				{
					var command = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(command);

					NextState = command;

					goto Cleanup;
				}

				var isUninjuredGroup = gDobjMonster.GroupCount > 1 && gDobjMonster.DmgTaken == 0;

				Globals.Buf.SetFormat("{0}{1} {2} ",
					Environment.NewLine,
					isUninjuredGroup ? "They" : gDobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
					isUninjuredGroup ? "are" : "is");

				gDobjMonster.AddHealthStatus(Globals.Buf);

				gOut.Write("{0}", Globals.Buf);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			gCommandParser.ParseName();

			ContainerType = Prep != null ? Prep.ContainerType : (ContainerType)(-1);

			if (string.Equals(gCommandParser.ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) || string.Equals(gCommandParser.ObjData.Name, "area", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<ILookCommand>();

				CopyCommandData(command);

				gCommandParser.NextState = command;
			}
			else
			{
				gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter() || a.IsInRoom(gActorRoom),
					a => a.IsEmbeddedInRoom(gActorRoom),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(gActorRoom, gEngine.ExposeContainersRecursively),
					a => a.IsWornByCharacter()
				};

				if (!Enum.IsDefined(typeof(ContainerType), ContainerType))
				{
					gCommandParser.ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };
				}

				gCommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				gCommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				gCommandParser.ObjData.MonsterNotFoundFunc = PrintYouSeeNothingSpecial;

				PlayerResolveArtifact();
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return Enum.IsDefined(typeof(ContainerType), ContainerType) && !artifact.IsWornByCharacter();
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			var prepNames = new string[] { "in", "into", "on", "onto", "under", "behind" };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public ExamineCommand()
		{
			SortOrder = 150;

			IsDobjPrepEnabled = true;

			Name = "ExamineCommand";

			Verb = "examine";

			Type = CommandType.Manipulation;
		}
	}
}
