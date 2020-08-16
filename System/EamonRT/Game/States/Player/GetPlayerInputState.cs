
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : State, IGetPlayerInputState
	{
		public override void Execute()
		{
			ProcessEvents(EventType.BeforeCommandPromptPrint);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			gOut.Write("{0}{1}", Environment.NewLine, Globals.CommandPrompt);

			Globals.CommandPromptSeen = true;

			Globals.CursorPosition = gOut.GetCursorPosition();

			if (Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= gOut.GetBufferHeight())
			{
				Globals.CursorPosition.Y--;
			}

			gOut.WriteLine();

			gOut.SetCursorPosition(Globals.CursorPosition);

			Globals.CommandParser.Clear();

			Globals.CommandParser.ActorMonster = gCharMonster;

			Globals.CommandParser.InputBuf.SetFormat("{0}", Globals.In.ReadLine());

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IProcessPlayerInputState>();
			}

			Globals.NextState = NextState;
		}

		public GetPlayerInputState()
		{
			Name = "GetPlayerInputState";
		}
	}
}
