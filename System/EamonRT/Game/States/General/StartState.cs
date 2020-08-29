
// StartState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StartState : State, IStartState
	{
		public override void Execute()
		{
			ProcessEvents(EventType.BeforeRoundStart);

			if (!Globals.CommandPromptSeen || ShouldPreTurnProcess())
			{
				gGameState.CurrTurn++;

				Debug.Assert(gGameState.CurrTurn > 0);
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBurnDownLightSourceState>();
			}

			Globals.NextState = NextState;
		}

		public StartState()
		{
			Name = "StartState";
		}
	}
}
