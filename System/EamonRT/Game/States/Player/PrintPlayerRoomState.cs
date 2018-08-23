
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using System.Text;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		/// <summary>
		/// This event fires before the player's room has been printed.
		/// </summary>
		public const long PeBeforePlayerRoomPrint = 1;

		public virtual void BuildPrintedTooDarkToSeeDesc(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			buf.SetPrint("It's too dark to see.");
		}

		public virtual void PrintPlayerRoom()
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
				BuildPrintedTooDarkToSeeDesc(Globals.Buf);
			}

			Globals.Out.Write("{0}", Globals.Buf);
		}

		public override void Execute()
		{
			ProcessEvents(PeBeforePlayerRoomPrint);

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

/* EamonCsCodeTemplate

// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{

	}
}
EamonCsCodeTemplate */
