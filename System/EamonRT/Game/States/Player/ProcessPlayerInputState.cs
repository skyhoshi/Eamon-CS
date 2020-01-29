
// ProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ProcessPlayerInputState : State, IProcessPlayerInputState
	{
		public const long PeAfterLastCommandListClear = 1;

		public virtual bool IncrementCurrTurn { get; set; }

		public override void Execute()
		{
			Globals.CommandParser.Execute();

			if (IncrementCurrTurn)
			{
				gGameState.CurrTurn++;

				Debug.Assert(gGameState.CurrTurn > 0);
			}

			Globals.LastCommandList.Clear();

			ProcessEvents(PeAfterLastCommandListClear);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CommandParser.NextState;

			Globals.CommandParser.Clear();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IUnrecognizedCommandState>();
			}

			Globals.NextState = NextState;
		}

		public ProcessPlayerInputState()
		{
			Name = "ProcessPlayerInputState";

			IncrementCurrTurn = true;
		}
	}
}
