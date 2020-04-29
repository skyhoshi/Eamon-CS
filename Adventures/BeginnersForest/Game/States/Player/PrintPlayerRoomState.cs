
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
				var spookMonster = gMDB[9];

				Debug.Assert(spookMonster != null);

				// Random Annoying Spooks (4 Spook limit)

				if (spookMonster.IsInLimbo() || spookMonster.IsInRoomUid(gGameState.Ro))
				{
					if (gGameState.Ro > 1 && gGameState.Ro < 5 && gGameState.SpookCounter < 10 && spookMonster.GroupCount < 4)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl < 35)
						{
							spookMonster.Seen = false;

							spookMonster.GroupCount++;

							spookMonster.InitGroupCount++;

							spookMonster.OrigGroupCount++;

							spookMonster.Location = gGameState.Ro;

							gGameState.SpookCounter++;
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
