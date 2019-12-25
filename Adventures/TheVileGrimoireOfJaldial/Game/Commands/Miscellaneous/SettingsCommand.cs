﻿
// SettingsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings(typeof(ISettingsCommand))]
	public class SettingsCommand : EamonRT.Game.Commands.SettingsCommand, Framework.Commands.ISettingsCommand
	{
		public virtual bool? ExitDirNames { get; set; }

		public override void PrintUsage()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			base.PrintUsage();

			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "ExitDirNames", "True, False", gameState.ExitDirNames);
		}

		public override void PlayerExecute()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (ExitDirNames == null)
			{
				base.PlayerExecute();

				goto Cleanup;
			}

			if (ExitDirNames != null)
			{
				gameState.ExitDirNames = (bool)ExitDirNames;
			}

			Globals.Out.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

		Cleanup:

			;
		}

		public override void PlayerFinishParsing()
		{
			bool boolValue = false;

			if (CommandParser.CurrToken + 1 < CommandParser.Tokens.Length)
			{
				if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "exitdirnames", StringComparison.OrdinalIgnoreCase) && bool.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out boolValue))
				{
					ExitDirNames = boolValue;

					CommandParser.CurrToken += 2;
				}
				else
				{
					base.PlayerFinishParsing();
				}
			}
			else
			{
				base.PlayerFinishParsing();
			}
		}
	}
}
