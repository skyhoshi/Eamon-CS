
// GetPlayerInputState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : State, IGetPlayerInputState
	{
		protected virtual void ProcessEvents()
		{

		}

		public override void Execute()
		{
			ProcessEvents();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.CommandPrompt);

			Globals.CursorPosition = Globals.Out.GetCursorPosition();

			if (Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= Globals.Out.GetBufferHeight())
			{
				Globals.CursorPosition.Y--;
			}

			Globals.Out.WriteLine();

			Globals.Out.SetCursorPosition(Globals.CursorPosition);

			Globals.CommandParser.Clear();

			Globals.CommandParser.ActorMonster = Globals.MDB[Globals.GameState.Cm];

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
