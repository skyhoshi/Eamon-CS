
// CommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void CheckPlayerCommand(ICommand command, bool afterFinishParsing)
		{
			Debug.Assert(command != null);

			// Restrict commands while climbing cliff

			if (afterFinishParsing && gActorRoom.Uid == 15 && !(command.Type == CommandType.Movement || command.Type == CommandType.Miscellaneous || command is IExamineCommand || command is IHealCommand || command is ISmileCommand))
			{
				gOut.Print("You're clinging to the side of a cliff!");

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.CheckPlayerCommand(command, afterFinishParsing);
			}
		}
	}
}
