
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
		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s full description has been printed (but before
		/// units are listed for drinkables/edibles).
		/// </summary>
		public const long PpeAfterArtifactFullDescPrint = 1;

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s container contents are printed.
		/// </summary>
		public const long PpeAfterArtifactContentsPrint = 2;

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjArtifact != null)
			{
				var artTypes = new ArtifactType[] { ArtifactType.DoorGate, ArtifactType.DisguisedMonster, ArtifactType.Drinkable, ArtifactType.Edible, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };

				var ac = DobjArtifact.GetArtifactCategory(artTypes, false);

				if (ac == null)
				{
					ac = DobjArtifact.GetCategories(0);
				}

				Debug.Assert(ac != null);

				if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					DobjArtifact.SetInRoom(ActorRoom);
				}

				if (ac.Type == ArtifactType.DoorGate)
				{
					ac.Field4 = 0;
				}

				if (ac.Type == ArtifactType.DisguisedMonster)
				{
					gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				Globals.Buf.Clear();

				if (Enum.IsDefined(typeof(ContainerType), ContainerType) && !DobjArtifact.IsWornByCharacter())
				{
					var containerArtType = gEngine.EvalContainerType(ContainerType, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer);

					var ac01 = DobjArtifact.GetArtifactCategory(containerArtType);

					if (ac01 == null)
					{
						PrintYouSeeNothingSpecial();

						goto Cleanup;
					}

					if (ac01 == DobjArtifact.InContainer && !ac01.IsOpen())
					{
						PrintMustFirstOpen(DobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					var artifactList = DobjArtifact.GetContainedList(containerType: ContainerType);
					
					var showCharOwned = !DobjArtifact.IsCarriedByCharacter() /* && !DobjArtifact.IsWornByCharacter() */;

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} you see ",
							Environment.NewLine,
							gEngine.EvalContainerType(ContainerType, "Inside", "On", "Under", "Behind"),
							DobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));

						var rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, showCharOwned, StateDescDisplayCode.None, false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.SetFormat("{0}There's nothing {1} {2}",
							Environment.NewLine,
							gEngine.EvalContainerType(ContainerType, "inside", "on", "under", "behind"),
							DobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));
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
					var rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Write("{0}", Globals.Buf);

					DobjArtifact.Seen = true;

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

					if (((ac.Type == ArtifactType.InContainer && ac.IsOpen()) || ac.Type == ArtifactType.OnContainer || ac.Type == ArtifactType.UnderContainer || ac.Type == ArtifactType.BehindContainer) && DobjArtifact.ShouldShowContentsWhenExamined())
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

				var rc = DobjMonster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Write("{0}", Globals.Buf);

				DobjMonster.Seen = true;

				if (DobjMonster.Friendliness == Friendliness.Friend && DobjMonster.ShouldShowContentsWhenExamined())
				{
					var command = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(command);

					NextState = command;

					goto Cleanup;
				}

				if (DobjMonster.ShouldShowHealthStatusWhenExamined())
				{
					var isUninjuredGroup = DobjMonster.GroupCount > 1 && DobjMonster.DmgTaken == 0;

					Globals.Buf.SetFormat("{0}{1} {2} ",
						Environment.NewLine,
						isUninjuredGroup ? "They" : DobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
						isUninjuredGroup ? "are" : "is");

					DobjMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
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
