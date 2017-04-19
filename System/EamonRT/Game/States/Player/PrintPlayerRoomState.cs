
// PrintPlayerRoomState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		protected virtual void ProcessEvents()
		{

		}

		protected virtual void PrintPlayerRoom()
		{
			var room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(room != null);

			if (room.IsLit())
			{
				Globals.Buf.Clear();

				var rc = room.BuildPrintedFullDesc(Globals.Buf, verboseRoomDesc: Globals.GameState.Vr, verboseMonsterDesc: Globals.GameState.Vm, verboseArtifactDesc: Globals.GameState.Va);

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}
			else
			{
				Globals.Buf.SetFormat("{0}It's too dark to see.{0}", Environment.NewLine);
			}

			Globals.Out.Write("{0}", Globals.Buf);
		}

		public override void Execute()
		{
			ProcessEvents();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintPlayerRoom();

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
