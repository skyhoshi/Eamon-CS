
// ProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Plugin;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ProcessPlayerInputState : State, IProcessPlayerInputState
	{
		/// <summary>
		/// An event that fires after the player's command has been processed (but not executed), 
		/// the <see cref="IGameState.CurrTurn">CurrTurn</see> counter incremented and the
		/// <see cref="IPluginGlobals.LastCommandList">LastCommandList</see> cleared.
		/// </summary>
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
