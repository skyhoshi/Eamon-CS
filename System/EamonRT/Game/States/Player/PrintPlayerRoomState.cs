
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		public override void Execute()
		{
			ProcessRevealContentArtifactList();

			ProcessEvents(EventType.BeforePlayerRoomPrint);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			gEngine.PrintPlayerRoom();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAfterPrintPlayerRoomEventState>();
			}

			Globals.NextState = NextState;
		}

		public PrintPlayerRoomState()
		{
			Name = "PrintPlayerRoomState";
		}
	}
}
