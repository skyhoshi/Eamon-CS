
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			//          Spook Reducer 2.0
			//  (c) 2012 Frank Black Productions

			if (eventType == PpeAfterPlayerSay && string.Equals(ProcessedPhrase, "less spooks", StringComparison.OrdinalIgnoreCase) && gameState.SpookCounter < 8)
			{
				var spookMonster = Globals.MDB[9];

				Debug.Assert(spookMonster != null);

				spookMonster.GroupCount = spookMonster.GroupCount > 0 ? 1 : 0;

				spookMonster.InitGroupCount = spookMonster.GroupCount;

				spookMonster.OrigGroupCount = spookMonster.GroupCount;

				gameState.SpookCounter = 8;

				Globals.Out.Print("Less spooks it is!");

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			base.PlayerProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
