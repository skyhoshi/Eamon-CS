
// GoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class GoCommand : EamonRT.Game.Commands.GoCommand, IGoCommand
	{
		public GoCommand()
		{
			// Synonyms can be applied to verbs as well

			Synonyms = new string[] { "enter" };
		}
	}
}
