
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : Command, IExamineCommand
	{
		/// <summary>
		/// This event fires after an artifact's full description has been printed (but before
		/// units are listed for drinkables/edibles).
		/// </summary>
		public const long PpeAfterArtifactFullDescPrint = 1;

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjArtifact != null)
			{
				var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.DoorGate, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Drinkable, Enums.ArtifactType.Edible, Enums.ArtifactType.Container };

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

				if (ac.Type == Enums.ArtifactType.DoorGate)
				{
					ac.Field4 = 0;
				}

				if (ac.Type == Enums.ArtifactType.DisguisedMonster)
				{
					Globals.Engine.RevealDisguisedMonster(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				Globals.Buf.Clear();

				var rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Write("{0}", Globals.Buf);

				DobjArtifact.Seen = true;

				PlayerProcessEvents(PpeAfterArtifactFullDescPrint);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if ((ac.Type == Enums.ArtifactType.Drinkable || ac.Type == Enums.ArtifactType.Edible) && ac.Field2 != Constants.InfiniteDrinkableEdible)
				{
					Globals.Out.Print("There {0}{1}{2}{3} left.",
						ac.Field2 != 1 ? "are " : "is ",
						ac.Field2 > 0 ? Globals.Engine.GetStringFromNumber(ac.Field2, false, Globals.Buf) : "no",
						ac.Type == Enums.ArtifactType.Drinkable ? " swallow" : " bite",
						ac.Field2 != 1 ? "s" : "");
				}

				if (ac.Type == Enums.ArtifactType.Container && ac.IsOpen() && DobjArtifact.ShouldShowContentsWhenExamined())
				{
					var command = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(command);

					NextState = command;

					goto Cleanup;
				}
			}
			else
			{
				Globals.Buf.Clear();

				var rc = DobjMonster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Write("{0}", Globals.Buf);

				DobjMonster.Seen = true;

				if (DobjMonster.Friendliness == Enums.Friendliness.Friend && DobjMonster.ShouldShowContentsWhenExamined())
				{
					var command = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(command);

					NextState = command;

					goto Cleanup;
				}

				var isUninjuredGroup = DobjMonster.GroupCount > 1 && DobjMonster.DmgTaken == 0;

				Globals.Buf.SetFormat("{0}{1} {2} ",
					Environment.NewLine,
					isUninjuredGroup ? "They" : DobjMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01),
					isUninjuredGroup ? "are" : "is");

				DobjMonster.AddHealthStatus(Globals.Buf);

				Globals.Out.Write("{0}", Globals.Buf);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			CommandParser.ParseName();

			if (string.Equals(CommandParser.ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) || string.Equals(CommandParser.ObjData.Name, "area", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<ILookCommand>();

				CopyCommandData(command);

				CommandParser.NextState.Discarded = true;

				CommandParser.NextState = command;
			}
			else
			{
				CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
					a => a.IsEmbeddedInRoom(ActorRoom),
					a => a.IsWornByCharacter()
				};

				CommandParser.ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };

				CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				CommandParser.ObjData.MonsterNotFoundFunc = PrintYouSeeNothingSpecial;

				PlayerResolveArtifact();
			}
		}

		public ExamineCommand()
		{
			SortOrder = 150;

			Name = "ExamineCommand";

			Verb = "examine";

			Type = Enums.CommandType.Manipulation;
		}
	}
}

/* EamonCsCodeTemplate

// ExamineCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{

	}
}
EamonCsCodeTemplate */
