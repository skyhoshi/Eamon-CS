
// PluginConstants.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace ARuncibleCargo.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{
		public virtual string SnapshotFileName { get; protected set; } = "SNAPSHOT_001.XML";
	}
}
