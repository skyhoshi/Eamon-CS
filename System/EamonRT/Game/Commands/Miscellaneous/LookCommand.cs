
// LookCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class LookCommand : Command, ILookCommand
	{
		protected override void PlayerExecute()
		{
			ActorRoom.Seen = false;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			if (CommandParser.CurrToken < CommandParser.Tokens.Length && !string.Equals(CommandParser.Tokens[CommandParser.CurrToken], "room", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<IExamineCommand>();

				CopyCommandData(command);

				CommandParser.NextState.Dispose();

				CommandParser.NextState = command;

				command.FinishParsing();
			}
		}

		public LookCommand()
		{
			SortOrder = 330;

			Name = "LookCommand";

			Verb = "look";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
