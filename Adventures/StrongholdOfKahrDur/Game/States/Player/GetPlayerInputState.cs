
// GetPlayerInputState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IGetPlayerInputState))]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		protected override void ProcessEvents()
		{
			if (ShouldPreTurnProcess())
			{
				var room = Globals.RDB[84];

				Debug.Assert(room != null);

				var monster = Globals.MDB[22];

				Debug.Assert(monster != null);

				// Flavor effects

				var rl = Globals.Engine.RollDice01(1, 100, 0);

				var r = 5 + 5 * (!room.Seen ? 1 : 0);

				if (rl < r && !monster.IsInLimbo())
				{
					rl = Globals.Engine.RollDice01(1, 5, 64);

					Globals.Engine.PrintEffectDesc(rl);
				}
			}

			base.ProcessEvents();
		}
	}
}
