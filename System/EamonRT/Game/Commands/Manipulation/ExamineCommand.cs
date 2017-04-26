
// ExamineCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

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
		protected virtual void PlayerProcessEvents()
		{

		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjArtifact != null)
			{
				var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.DoorGate, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Drinkable, Enums.ArtifactType.Edible };

				var ac = DobjArtifact.GetArtifactClass(artClasses, false);

				if (ac == null)
				{
					ac = DobjArtifact.GetClasses(0);
				}

				Debug.Assert(ac != null);

				if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					DobjArtifact.SetInRoom(ActorRoom);
				}

				if (ac.Type == Enums.ArtifactType.DoorGate)
				{
					ac.Field8 = 0;
				}

				if (ac.Type == Enums.ArtifactType.DisguisedMonster)
				{
					Globals.RtEngine.RevealDisguisedMonster(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				Globals.Buf.Clear();

				var rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Write("{0}", Globals.Buf);

				DobjArtifact.Seen = true;

				PlayerProcessEvents();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if ((ac.Type == Enums.ArtifactType.Drinkable || ac.Type == Enums.ArtifactType.Edible) && ac.Field6 != Constants.InfiniteDrinkableEdible)
				{
					Globals.Out.Write("{0}There {1}{2}{3}{4} left.{0}",
						Environment.NewLine,
						ac.Field6 != 1 ? "are " : "is ",
						ac.Field6 > 0 ? Globals.Engine.GetStringFromNumber(ac.Field6, false, Globals.Buf) : "no",
						ac.Type == Enums.ArtifactType.Drinkable ? " swallow" : " bite",
						ac.Field6 != 1 ? "s" : "");
				}
			}
			else
			{
				Globals.Buf.Clear();

				var rc = DobjMonster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Write("{0}", Globals.Buf);

				DobjMonster.Seen = true;

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

		protected override void PlayerFinishParsing()
		{
			CommandParser.ParseName();

			if (string.Equals(CommandParser.ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) || string.Equals(CommandParser.ObjData.Name, "area", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<ILookCommand>();

				CopyCommandData(command);

				CommandParser.NextState.Dispose();

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
