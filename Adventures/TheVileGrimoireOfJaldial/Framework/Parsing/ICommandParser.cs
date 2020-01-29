
// ICommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Parsing
{
	public interface ICommandParser : EamonRT.Framework.Parsing.ICommandParser
	{
		long DecorationId { get; set; }
	}
}
