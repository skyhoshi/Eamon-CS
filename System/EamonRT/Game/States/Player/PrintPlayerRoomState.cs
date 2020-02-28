
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		/// <summary>
		/// An event that fires before the player's <see cref="IRoom">Room</see> has been printed.
		/// </summary>
		public const long PeBeforePlayerRoomPrint = 1;

		public override void Execute()
		{
			ProcessRevealContentArtifacts();

			ProcessEvents(PeBeforePlayerRoomPrint);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			gEngine.PrintPlayerRoom();

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
