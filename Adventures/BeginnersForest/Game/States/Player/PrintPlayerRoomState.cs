
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		protected override void ProcessEvents()
		{
			if (ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as Framework.IGameState;

				Debug.Assert(gameState != null);

				var monster = Globals.MDB[9];

				Debug.Assert(monster != null);

				// Random Annoying Spooks (4 Spook limit)

				if (gameState.Ro > 1 && gameState.Ro < 5 && gameState.SpookCounter < 10 && monster.GroupCount < 4)
				{
					var rl = Globals.Engine.RollDice01(1, 100, 0);

					if (rl < 35)
					{
						monster.Seen = false;

						monster.GroupCount++;

						monster.InitGroupCount++;

						monster.OrigGroupCount++;

						monster.Location = gameState.Ro;

						gameState.SpookCounter++;

						Globals.Engine.CheckEnemies();
					}
				}
			}

			base.ProcessEvents();
		}
	}
}
