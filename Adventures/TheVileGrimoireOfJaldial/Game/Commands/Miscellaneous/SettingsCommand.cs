
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

		public virtual long? WeatherScalePct { get; set; }

		public virtual long? EncounterScalePct { get; set; }

		public virtual long? FlavorScalePct { get; set; }

		public override void PrintUsage()
		{
			base.PrintUsage();

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "WeatherScalePct", "0 .. 100", gGameState.WeatherScalePct);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "EncounterScalePct", "0 .. 100", gGameState.EncounterScalePct);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "FlavorScalePct", "0 .. 100", gGameState.FlavorScalePct);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowCombatDamage", "True, False", gGameState.ShowCombatDamage);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ExitDirNames", "True, False", gGameState.ExitDirNames);
		}

		public override void PlayerExecute()
		{
			if (ShowCombatDamage == null && ExitDirNames == null && WeatherScalePct == null && EncounterScalePct == null && FlavorScalePct == null)
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

			if (WeatherScalePct != null)
			{
				Debug.Assert(WeatherScalePct >= 0 && WeatherScalePct <= 100);

				gGameState.WeatherScalePct = (long)WeatherScalePct;
			}

			if (EncounterScalePct != null)
			{
				Debug.Assert(EncounterScalePct >= 0 && EncounterScalePct <= 100);

				gGameState.EncounterScalePct = (long)EncounterScalePct;
			}

			if (FlavorScalePct != null)
			{
				Debug.Assert(FlavorScalePct >= 0 && FlavorScalePct <= 100);

				gGameState.FlavorScalePct = (long)FlavorScalePct;
			}

			gOut.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

		Cleanup:

			;
		}
	}
}
