
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Commands
{
	public interface ISettingsCommand : EamonRT.Framework.Commands.ISettingsCommand
	{
		bool? ExitDirNames { get; set; }
	}
}
