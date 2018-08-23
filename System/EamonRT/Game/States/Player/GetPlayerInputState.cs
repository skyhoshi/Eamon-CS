
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		/// <summary>
		/// This event fires before the player's command prompt is printed.
		/// </summary>
		public const long PeBeforeCommandPromptPrint = 1;

		public override void Execute()
		{
			ProcessEvents(PeBeforeCommandPromptPrint);

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

/* EamonCsCodeTemplate

// GetPlayerInputState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{

	}
}
EamonCsCodeTemplate */
