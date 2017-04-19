
// PluginConstants.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using TheBeginnersCave.Framework.Plugin;

namespace TheBeginnersCave.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, IPluginConstants
	{
		public virtual string AlightDesc { get; protected set; } = "(alight)";
	}
}
