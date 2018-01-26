
// VerboseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class VerboseCommand : Command, IVerboseCommand
	{
		public virtual bool VerboseRooms { get; set; }

		public virtual bool VerboseArtifacts { get; set; }

		public virtual bool VerboseMonsters { get; set; }

		protected override void PlayerExecute()
		{
			var changed = false;

			if (VerboseRooms)
			{
				Globals.GameState.Vr = !Globals.GameState.Vr;

				Globals.Out.Write("{0}Verbose mode {1} for rooms.", Environment.NewLine, Globals.GameState.Vr ? "enabled" : "disabled");

				changed = true;
			}

			if (VerboseArtifacts)
			{
				Globals.GameState.Va = !Globals.GameState.Va;

				Globals.Out.Write("{0}Verbose mode {1} for artifacts.", Environment.NewLine, Globals.GameState.Va ? "enabled" : "disabled");

				changed = true;
			}

			if (VerboseMonsters)
			{
				Globals.GameState.Vm = !Globals.GameState.Vm;

				Globals.Out.Write("{0}Verbose mode {1} for monsters.", Environment.NewLine, Globals.GameState.Vm ? "enabled" : "disabled");

				changed = true;
			}

			if (!changed)
			{
				Globals.Out.Write("{0}You can use this command for rooms, artifacts, monsters or all.", Environment.NewLine);
			}

			Globals.Out.WriteLine();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			if (CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "all", StringComparison.OrdinalIgnoreCase) || CommandParser.Tokens[CommandParser.CurrToken].StartsWith("room", StringComparison.OrdinalIgnoreCase))
				{
					VerboseRooms = true;
				}

				if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "all", StringComparison.OrdinalIgnoreCase) || CommandParser.Tokens[CommandParser.CurrToken].StartsWith("artifact", StringComparison.OrdinalIgnoreCase))
				{
					VerboseArtifacts = true;
				}

				if (string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "all", StringComparison.OrdinalIgnoreCase) || CommandParser.Tokens[CommandParser.CurrToken].StartsWith("monster", StringComparison.OrdinalIgnoreCase))
				{
					VerboseMonsters = true;
				}

				CommandParser.CurrToken++;
			}
			else
			{
				VerboseRooms = true;
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public VerboseCommand()
		{
			SortOrder = 400;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "VerboseCommand";

			Verb = "verbose";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
