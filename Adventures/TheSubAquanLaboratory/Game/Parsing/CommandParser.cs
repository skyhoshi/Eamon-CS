
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public virtual void PlayerFinishParsingPushCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingSearchCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingTurnCommand()
		{
			PlayerResolveRecord(false);
		}
	}
}
