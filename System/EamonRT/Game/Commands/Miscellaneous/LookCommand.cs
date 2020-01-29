
// LookCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class LookCommand : Command, ILookCommand
	{
		public override void PlayerExecute()
		{
			gActorRoom.Seen = false;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				var command = Globals.CreateInstance<IExamineCommand>();

				CopyCommandData(command);

				gCommandParser.NextState = command;

				command.FinishParsing();
			}
		}

		public LookCommand()
		{
			SortOrder = 330;

			Name = "LookCommand";

			Verb = "look";

			Type = CommandType.Miscellaneous;
		}
	}
}
