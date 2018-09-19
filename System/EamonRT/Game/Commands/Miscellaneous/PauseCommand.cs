
// PauseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PauseCommand : Command, IPauseCommand
	{
		public virtual long PauseMs { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(PauseMs >= 0 && PauseMs <= 10000);

			Globals.Out.Print("PAUSE time during combat:  {0} milliseconds.", PauseMs);

			Globals.GameState.PauseCombatMs = PauseMs;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			long tempValue = 0;

			if (CommandParser.CurrToken + 1 < CommandParser.Tokens.Length && string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "combat", StringComparison.OrdinalIgnoreCase) && long.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out tempValue) && tempValue >= 0 && tempValue <= 10000)
			{
				PauseMs = tempValue;

				CommandParser.CurrToken += 2;
			}
			else
			{
				Globals.Out.Print("Usage: PAUSE Combat [Milliseconds] where 0 <= Milliseconds <= 10000.");

				CommandParser.NextState.Discarded = true;

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public PauseCommand()
		{
			SortOrder = 405;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "PauseCommand";

			Verb = "pause";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
