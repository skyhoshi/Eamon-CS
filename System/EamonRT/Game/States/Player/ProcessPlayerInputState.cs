
// ProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ProcessPlayerInputState : State, IProcessPlayerInputState
	{
		public override void Execute()
		{
			Globals.CommandParser.Execute();

			Globals.LastCommandList.Clear();

			ProcessEvents(EventType.AfterLastCommandListClear);

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
		}
	}
}
