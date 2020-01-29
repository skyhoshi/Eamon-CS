
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual bool MonsterCurses { get; set; }

		public virtual bool DeviceOpened { get; set; }
	}
}
