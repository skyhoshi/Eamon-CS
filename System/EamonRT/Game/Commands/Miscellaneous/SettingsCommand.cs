
// SettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SettingsCommand : Command, ISettingsCommand
	{
		public virtual bool? VerboseRooms { get; set; } = null;

		public virtual bool? VerboseMonsters { get; set; } = null;

		public virtual bool? VerboseArtifacts { get; set; } = null;

		public virtual bool? MatureContent { get; set; } = null;

		public virtual long? PauseCombatMs { get; set; } = null;

		public override void PlayerExecute()
		{
			Debug.Assert(VerboseRooms != null || VerboseMonsters != null || VerboseArtifacts != null || MatureContent != null || PauseCombatMs != null);

			if (VerboseRooms != null)
			{
				gGameState.Vr = (bool)VerboseRooms;
			}

			if (VerboseMonsters != null)
			{
				gGameState.Vm = (bool)VerboseMonsters;
			}

			if (VerboseArtifacts != null)
			{
				gGameState.Va = (bool)VerboseArtifacts;
			}

			if (MatureContent != null)
			{
				gGameState.MatureContent = (bool)MatureContent;
			}

			if (PauseCombatMs != null)
			{
				Debug.Assert(PauseCombatMs >= 0 && PauseCombatMs <= 10000);

				gGameState.PauseCombatMs = (long)PauseCombatMs;
			}

			gOut.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public virtual void PrintUsage()
		{
			gOut.Print("Usage:  SETTINGS [Option] [Value]{0}", Environment.NewLine);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "Option", "Value", "Setting");
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "------", "-----", "-------");
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseRooms", "True, False", gGameState.Vr);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseMonsters", "True, False", gGameState.Vm);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseArtifacts", "True, False", gGameState.Va);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "MatureContent", "True, False", gGameState.MatureContent);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "PauseCombatMs", "0 .. 10000", gGameState.PauseCombatMs);
		}

		public SettingsCommand()
		{
			SortOrder = 400;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "SettingsCommand";

			Verb = "settings";

			Type = CommandType.Miscellaneous;
		}
	}
}
