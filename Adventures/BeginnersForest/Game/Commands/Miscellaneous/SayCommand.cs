
// SayCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using BeginnersForest.Framework;
using BeginnersForest.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ISayCommand))]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		protected override void PlayerProcessEvents01()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			//          Spook Reducer 2.0
			//  (c) 2012 Frank Black Productions

			if (string.Equals(ProcessedPhrase, "less spooks", StringComparison.OrdinalIgnoreCase) && gameState.SpookCounter < 8)
			{
				var monster = Globals.MDB[9];

				Debug.Assert(monster != null);

				monster.GroupCount = monster.GroupCount > 0 ? 1 : 0;

				monster.InitGroupCount = monster.GroupCount;

				monster.OrigGroupCount = monster.GroupCount;

				Globals.Engine.CheckEnemies();

				gameState.SpookCounter = 8;

				Globals.Out.WriteLine("{0}Less spooks it is!", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				goto Cleanup;
			}

			base.PlayerProcessEvents01();

		Cleanup:

			;
		}
	}
}
