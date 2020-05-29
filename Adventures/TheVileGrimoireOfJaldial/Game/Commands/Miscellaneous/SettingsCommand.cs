
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
		public virtual bool? ShowCombatDamage { get; set; }

		public virtual bool? ExitDirNames { get; set; }

		public override void PrintUsage()
		{
			base.PrintUsage();

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowCombatDamage", "True, False", gGameState.ShowCombatDamage);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ExitDirNames", "True, False", gGameState.ExitDirNames);
		}

		public override void PlayerExecute()
		{
			if (ShowCombatDamage == null && ExitDirNames == null)
			{
				base.PlayerExecute();

				goto Cleanup;
			}

			if (ShowCombatDamage != null)
			{
				gGameState.ShowCombatDamage = (bool)ShowCombatDamage;
			}

			if (ExitDirNames != null)
			{
				gGameState.ExitDirNames = (bool)ExitDirNames;
			}

			gOut.Print("Settings changed.");

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

			if (gCommandParser.CurrToken + 1 < gCommandParser.Tokens.Length)
			{
				if (string.Equals(gCommandParser.Tokens[gCommandParser.CurrToken], "showcombatdamage", StringComparison.OrdinalIgnoreCase) && bool.TryParse(gCommandParser.Tokens[gCommandParser.CurrToken + 1], out boolValue))
				{
					ShowCombatDamage = boolValue;

					gCommandParser.CurrToken += 2;
				}
				else if (string.Equals(gCommandParser.Tokens[gCommandParser.CurrToken], "exitdirnames", StringComparison.OrdinalIgnoreCase) && bool.TryParse(gCommandParser.Tokens[gCommandParser.CurrToken + 1], out boolValue))
				{
					ExitDirNames = boolValue;

					gCommandParser.CurrToken += 2;
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
