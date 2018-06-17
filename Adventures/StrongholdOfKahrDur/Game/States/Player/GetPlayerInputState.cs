
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeCommandPromptPrint && ShouldPreTurnProcess())
			{
				var room = Globals.RDB[84];

				Debug.Assert(room != null);

				var necromancerMonster = Globals.MDB[22];

				Debug.Assert(necromancerMonster != null);

				// Flavor effects

				var rl = Globals.Engine.RollDice01(1, 100, 0);

				var r = 5 + 5 * (!room.Seen ? 1 : 0);

				if (rl < r && !necromancerMonster.IsInLimbo())
				{
					rl = Globals.Engine.RollDice01(1, 5, 64);

					Globals.Engine.PrintEffectDesc(rl);
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
