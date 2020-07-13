
// StartState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StartState : State, IStartState
	{
		/// <summary>
		/// An event that fires at the start of a new round, before any processing has been done.
		/// </summary>
		public const long PeBeforeRoundStart = 1;

		public override void Execute()
		{
			ProcessRevealContentArtifacts();

			ProcessEvents(PeBeforeRoundStart);

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
