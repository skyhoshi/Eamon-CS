
// ScoreCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class ScoreCommand : EamonRT.Game.Commands.Command, IScoreCommand
	{
		protected override void PlayerExecute()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var artifact = Globals.ADB[9];

			Debug.Assert(artifact != null);

			if (artifact.Seen)
			{
				Globals.Out.Write("{0}Read bronze plaque: {1}", Environment.NewLine, gameState.ReadPlaque ? "Yes" : "No");
			}

			artifact = Globals.ADB[48];

			Debug.Assert(artifact != null);

			if (artifact.Seen)
			{
				Globals.Out.Write("{0}Read display screen: {1}", Environment.NewLine, gameState.ReadDisplayScreen ? "Yes" : "No");
			}

			artifact = Globals.ADB[50];

			Debug.Assert(artifact != null);

			if (artifact.Seen)
			{
				Globals.Out.Write("{0}Read computer terminals: {1}", Environment.NewLine, gameState.ReadTerminals ? "Yes" : "No");
			}

			var rooms = Globals.Database.RoomTable.Records.Where(r => r.Zone == 2).ToList();

			var seenCount = rooms.Count(r => r.Seen);

			Globals.Out.Write("{0}{1}/{2} laboratory rooms explored.{0}", Environment.NewLine, seenCount, rooms.Count);

			var percent = gameState.ReadPlaque ? 25L : 0L;

			percent += (gameState.ReadDisplayScreen ? 25L : 0L);

			percent += (gameState.ReadTerminals ? 25L : 0L);

			percent += (long)Math.Round(((double)seenCount / (double)rooms.Count) * 25);

			Globals.Out.Write("{0}{1}% of your quest is complete.{0}", Environment.NewLine, percent);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
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

			IsMonsterEnabled = false;

			Name = "ScoreCommand";

			Verb = "score";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
