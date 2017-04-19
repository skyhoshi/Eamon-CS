
// PluginConstants.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using EamonDD.Framework.Plugin;

namespace EamonDD.Game.Plugin
{
	public class PluginConstants : Eamon.Game.Plugin.PluginConstants, IPluginConstants
	{
		public virtual string DdProgVersion { get; protected set; }

		public PluginConstants()
		{
			DdProgVersion = ProgVersion;
		}
	}
}
