
// SettingsCommand.cs

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
	public class SettingsCommand : Command, ISettingsCommand
	{
		public virtual bool? VerboseRooms { get; set; } = null;

		public virtual bool? VerboseMonsters { get; set; } = null;

		public virtual bool? VerboseArtifacts { get; set; } = null;

		public virtual long? PauseCombatMs { get; set; } = null;

		protected virtual void PrintUsage()
		{
			Globals.Out.Print("Usage:  SETTINGS [Option] [Value]{0}", Environment.NewLine);

			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "Option", "Value", "Setting");
			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "------", "-----", "-------");
			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseRooms", "True, False", Globals.GameState.Vr);
			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseMonsters", "True, False", Globals.GameState.Vm);
			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseArtifacts", "True, False", Globals.GameState.Va);
			Globals.Out.WriteLine("  {0,-22}{1,-22}{2,-22}", "PauseCombatMs", "0 .. 10000", Globals.GameState.PauseCombatMs);
		}

		public override void PlayerExecute()
		{
			Debug.Assert(VerboseRooms != null || VerboseMonsters != null || VerboseArtifacts != null || PauseCombatMs != null);

			if (VerboseRooms != null)
			{
				Globals.GameState.Vr = (bool)VerboseRooms;
			}

			if (VerboseMonsters != null)
			{
				Globals.GameState.Vm = (bool)VerboseMonsters;
			}

			if (VerboseArtifacts != null)
			{
				Globals.GameState.Va = (bool)VerboseArtifacts;
			}

			if (PauseCombatMs != null)
			{
				Debug.Assert(PauseCombatMs >= 0 && PauseCombatMs <= 10000);

				Globals.GameState.PauseCombatMs = (long)PauseCombatMs;
			}

			Globals.Out.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			long longValue = 0;

			bool boolValue = false;

			if (CommandParser.CurrToken + 1 < CommandParser.Tokens.Length)
			{
				if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "verboserooms", StringComparison.OrdinalIgnoreCase) && bool.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out boolValue))
				{
					VerboseRooms = boolValue;

					CommandParser.CurrToken += 2;
				}
				else if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "verbosemonsters", StringComparison.OrdinalIgnoreCase) && bool.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out boolValue))
				{
					VerboseMonsters = boolValue;

					CommandParser.CurrToken += 2;
				}
				else if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "verboseartifacts", StringComparison.OrdinalIgnoreCase) && bool.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out boolValue))
				{
					VerboseArtifacts = boolValue;

					CommandParser.CurrToken += 2;
				}
				else if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "pausecombatms", StringComparison.OrdinalIgnoreCase) && long.TryParse(CommandParser.Tokens[CommandParser.CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 10000)
				{
					PauseCombatMs = longValue;

					CommandParser.CurrToken += 2;
				}
				else
				{
					PrintUsage();

					CommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				PrintUsage();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public SettingsCommand()
		{
			SortOrder = 400;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "SettingsCommand";

			Verb = "settings";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
