﻿
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : Command, IExamineCommand
	{
		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> ContainerArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtContainerAc { get; set; }

		/// <summary></summary>
		public virtual ArtifactType ContainerArtType { get; set; }

		/// <summary></summary>
		public virtual ICommand ArtifactInventoryCommand { get; set; }

		/// <summary></summary>
		public virtual ICommand MonsterInventoryCommand { get; set; }

		/// <summary></summary>
		public virtual bool ShowCharOwned { get; set; }

		/// <summary></summary>
		public virtual bool IsUninjuredGroupMonster { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjArtifact != null)
			{
				DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

				if (DobjArtAc == null)
				{
					DobjArtAc = DobjArtifact.GetCategories(0);
				}

				Debug.Assert(DobjArtAc != null);

				if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					DobjArtifact.SetInRoom(ActorRoom);
				}

				if (DobjArtAc.Type == ArtifactType.DoorGate)
				{
					DobjArtAc.Field4 = 0;
				}

				if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
				{
					gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				Globals.Buf.Clear();

				if (Enum.IsDefined(typeof(ContainerType), ContainerType) && !DobjArtifact.IsWornByCharacter())
				{
					ContainerArtType = gEngine.EvalContainerType(ContainerType, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer);

					DobjArtContainerAc = DobjArtifact.GetArtifactCategory(ContainerArtType);

					if (DobjArtContainerAc == null)
					{
						PrintYouSeeNothingSpecial();

						goto Cleanup;
					}

					if (DobjArtContainerAc == DobjArtifact.InContainer && !DobjArtContainerAc.IsOpen())
					{
						PrintMustFirstOpen(DobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					ContainerArtifactList = DobjArtifact.GetContainedList(containerType: ContainerType);
					
					ShowCharOwned = !DobjArtifact.IsCarriedByCharacter() /* && !DobjArtifact.IsWornByCharacter() */;

					if (ContainerArtifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} you see ",
							Environment.NewLine,
							gEngine.EvalContainerType(ContainerType, "Inside", "On", "Under", "Behind"),
							DobjArtifact.GetTheName(false, ShowCharOwned, false, false, Globals.Buf01));

						rc = gEngine.GetRecordNameList(ContainerArtifactList.Cast<IGameBase>().ToList(), ArticleType.A, ShowCharOwned, StateDescDisplayCode.None, false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.SetFormat("{0}There's nothing {1} {2}",
							Environment.NewLine,
							gEngine.EvalContainerType(ContainerType, "inside", "on", "under", "behind"),
							DobjArtifact.GetTheName(false, ShowCharOwned, false, false, Globals.Buf01));
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					gOut.Write("{0}", Globals.Buf);

					PlayerProcessEvents(EventType.AfterArtifactContentsPrint);

					if (GotoCleanup)
					{
						goto Cleanup;
					}
				}
				else
				{
					rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Write("{0}", Globals.Buf);

					DobjArtifact.Seen = true;

					PlayerProcessEvents(EventType.AfterArtifactFullDescPrint);

					if (GotoCleanup)
					{
						goto Cleanup;
					}

					if ((DobjArtAc.Type == ArtifactType.Drinkable || DobjArtAc.Type == ArtifactType.Edible) && DobjArtAc.Field2 != Constants.InfiniteDrinkableEdible)
					{
						gOut.Print("There {0}{1}{2}{3} left.",
							DobjArtAc.Field2 != 1 ? "are " : "is ",
							DobjArtAc.Field2 > 0 ? gEngine.GetStringFromNumber(DobjArtAc.Field2, false, Globals.Buf) : "no",
							DobjArtAc.Type == ArtifactType.Drinkable ? " swallow" : " bite",
							DobjArtAc.Field2 != 1 ? "s" : "");
					}

					if (((DobjArtAc.Type == ArtifactType.InContainer && DobjArtAc.IsOpen()) || DobjArtAc.Type == ArtifactType.OnContainer || DobjArtAc.Type == ArtifactType.UnderContainer || DobjArtAc.Type == ArtifactType.BehindContainer) && DobjArtifact.ShouldShowContentsWhenExamined())
					{
						ArtifactInventoryCommand = Globals.CreateInstance<IInventoryCommand>(x =>
						{
							x.AllowExtendedContainers = true;
						});

						CopyCommandData(ArtifactInventoryCommand);

						NextState = ArtifactInventoryCommand;

						goto Cleanup;
					}
				}
			}
			else
			{
				Globals.Buf.Clear();

				rc = DobjMonster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Write("{0}", Globals.Buf);

				DobjMonster.Seen = true;

				if (DobjMonster.Friendliness == Friendliness.Friend && DobjMonster.ShouldShowContentsWhenExamined())
				{
					MonsterInventoryCommand = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(MonsterInventoryCommand);

					NextState = MonsterInventoryCommand;

					goto Cleanup;
				}

				if (DobjMonster.ShouldShowHealthStatusWhenExamined())
				{
					IsUninjuredGroupMonster = DobjMonster.GroupCount > 1 && DobjMonster.DmgTaken == 0;

					Globals.Buf.SetFormat("{0}{1} {2} ",
						Environment.NewLine,
						IsUninjuredGroupMonster ? "They" : DobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
						IsUninjuredGroupMonster ? "are" : "is");

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

			PrepNames = new string[] { "in", "into", "on", "onto", "under", "behind" };

			return PrepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public ExamineCommand()
		{
			SortOrder = 150;

			IsDobjPrepEnabled = true;

			Name = "ExamineCommand";

			Verb = "examine";

			Type = CommandType.Manipulation;

			ArtTypes = new ArtifactType[] { ArtifactType.DoorGate, ArtifactType.DisguisedMonster, ArtifactType.Drinkable, ArtifactType.Edible, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };
		}
	}
}
