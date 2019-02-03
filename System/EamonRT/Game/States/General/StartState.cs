
// StartState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StartState : State, IStartState
	{
		public const long PeBeforeRoundStart = 1;

		public override void Execute()
		{
			ProcessEvents(PeBeforeRoundStart);

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
