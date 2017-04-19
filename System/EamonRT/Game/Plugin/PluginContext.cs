
// PluginContext.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using EamonRT.Framework.Plugin;

namespace EamonRT.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants
		{
			get
			{
				return (IPluginConstants)EamonDD.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonDD.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static IPluginClassMappings ClassMappings
		{
			get
			{
				return (IPluginClassMappings)EamonDD.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonDD.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static IPluginGlobals Globals
		{
			get
			{
				return (IPluginGlobals)EamonDD.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonDD.Game.Plugin.PluginContext.Globals = value;
			}
		}
	}
}
