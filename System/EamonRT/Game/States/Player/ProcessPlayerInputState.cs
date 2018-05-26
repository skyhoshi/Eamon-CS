
// ProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ProcessPlayerInputState : State, IProcessPlayerInputState
	{
		/// <summary>
		/// This event fires after the player's command has been processed (but not executed), 
		/// the current turn counter incremented and the last command list cleared.
		/// </summary>
		protected const long PeAfterLastCommandListClear = 1;

		public virtual bool IncrementCurrTurn { get; set; }

		public override void Execute()
		{
			Globals.CommandParser.Execute();

			if (IncrementCurrTurn)
			{
				Globals.GameState.CurrTurn++;

				Debug.Assert(Globals.GameState.CurrTurn > 0);
			}

			while (Globals.LastCommandList.Count > 0)
			{
				Globals.LastCommandList[0].Dispose();

				Globals.LastCommandList.RemoveAt(0);
			}

			ProcessEvents(PeAfterLastCommandListClear);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CommandParser.NextState;

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
