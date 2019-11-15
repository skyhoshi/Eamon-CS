
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Framework.Commands
{
	public interface ISettingsCommand : EamonRT.Framework.Commands.ISettingsCommand
	{
		bool? ExplicitContent { get; set; }
	}
}