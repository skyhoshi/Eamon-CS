
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterExtinguishLightSourceCheck)
			{
				var swampRooms = new long[] { 42, 43, 44, 45 };

				// Check for travel through the swamp

				if (gGameState.Ro != 19 && (swampRooms.Contains(gGameState.R3) || swampRooms.Contains(gGameState.Ro)) && gEngine.RollDice(1, 100, 0) > 70)
				{
					if (!gEngine.SaveThrow(0))
					{
						gOut.Print("Unfortunately, you hit an unseen pitfall, and the swamp swallows you up.");

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
					else
					{
						gOut.Print("Unfortunately, you hit an unseen pitfall.  But just as you thought you were doomed, you find the fortitude to pull yourself out of it.");
					}
				}
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
