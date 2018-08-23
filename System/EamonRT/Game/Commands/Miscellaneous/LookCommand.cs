
// LookCommand.cs

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
	public class LookCommand : Command, ILookCommand
	{
		public override void PlayerExecute()
		{
			ActorRoom.Seen = false;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				var command = Globals.CreateInstance<IExamineCommand>();

				CopyCommandData(command);

				CommandParser.NextState.Discarded = true;

				CommandParser.NextState = command;

				command.FinishParsing();

				if (command.Discarded)
				{
					command.Dispose();
				}
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

/* EamonCsCodeTemplate

// LookCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{

	}
}
EamonCsCodeTemplate */
