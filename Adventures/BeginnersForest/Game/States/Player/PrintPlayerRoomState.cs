
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
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforePlayerRoomPrint && ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as Framework.IGameState;

				Debug.Assert(gameState != null);

				var spookMonster = Globals.MDB[9];

				Debug.Assert(spookMonster != null);

				// Random Annoying Spooks (4 Spook limit)

				if (gameState.Ro > 1 && gameState.Ro < 5 && gameState.SpookCounter < 10 && spookMonster.GroupCount < 4)
				{
					var rl = Globals.Engine.RollDice01(1, 100, 0);

					if (rl < 35)
					{
						spookMonster.Seen = false;

						spookMonster.GroupCount++;

						spookMonster.InitGroupCount++;

						spookMonster.OrigGroupCount++;

						spookMonster.Location = gameState.Ro;

						gameState.SpookCounter++;

						Globals.Engine.CheckEnemies();
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
