
// ScoreCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class ScoreCommand : EamonRT.Game.Commands.Command, Framework.Commands.IScoreCommand
	{
		public override void PlayerExecute()
		{
			var plaqueArtifact = gADB[9];

			Debug.Assert(plaqueArtifact != null);

			if (plaqueArtifact.Seen)
			{
				gOut.Write("{0}Read bronze plaque: {1}", Environment.NewLine, gGameState.ReadPlaque ? "Yes" : "No");
			}

			var displayScreenArtifact = gADB[48];

			Debug.Assert(displayScreenArtifact != null);

			if (displayScreenArtifact.Seen)
			{
				gOut.Write("{0}Read display screen: {1}", Environment.NewLine, gGameState.ReadDisplayScreen ? "Yes" : "No");
			}

			var terminalsArtifact = gADB[50];

			Debug.Assert(terminalsArtifact != null);

			if (terminalsArtifact.Seen)
			{
				gOut.Write("{0}Read computer terminals: {1}", Environment.NewLine, gGameState.ReadTerminals ? "Yes" : "No");
			}

			var rooms = Globals.Database.RoomTable.Records.Where(r => r.Zone == 2).ToList();

			var seenCount = rooms.Count(r => r.Seen);

			gOut.Print("{0}/{1} laboratory rooms explored.", seenCount, rooms.Count);

			var percent = gGameState.ReadPlaque ? 25L : 0L;

			percent += (gGameState.ReadDisplayScreen ? 25L : 0L);

			percent += (gGameState.ReadTerminals ? 25L : 0L);

			percent += (long)Math.Round(((double)seenCount / (double)rooms.Count) * 25);

			gOut.Print("{0}% of your quest is complete.", percent);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public ScoreCommand()
		{
			SortOrder = 470;

			IsNew = true;

			IsSentenceParserEnabled = false;

			IsMonsterEnabled = false;

			Uid = 88;

			Name = "ScoreCommand";

			Verb = "score";

			Type = CommandType.Miscellaneous;
		}
	}
}
