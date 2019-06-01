
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		public const long PeBeforePlayerRoomPrint = 1;

		public override void Execute()
		{
			ProcessEvents(PeBeforePlayerRoomPrint);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			Globals.Engine.PrintPlayerRoom();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IGetPlayerInputState>();
			}

			Globals.NextState = NextState;
		}

		public PrintPlayerRoomState()
		{
			Name = "PrintPlayerRoomState";
		}
	}
}
