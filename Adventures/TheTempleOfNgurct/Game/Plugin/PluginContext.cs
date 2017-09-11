
// PluginContext.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using TheTempleOfNgurct.Framework.Plugin;

namespace TheTempleOfNgurct.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants
		{
			get
			{
				return (IPluginConstants)EamonRT.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static IPluginClassMappings ClassMappings
		{
			get
			{
				return (IPluginClassMappings)EamonRT.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static IPluginGlobals Globals
		{
			get
			{
				return (IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}
	}
}
