
// PluginContext.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

/* EamonCsCodeTemplate

// PluginContext.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Game.Plugin
{
	public static class PluginContext
	{
		public static Framework.Plugin.IPluginConstants Constants
		{
			get
			{
				return (Framework.Plugin.IPluginConstants)EamonRT.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static Framework.Plugin.IPluginClassMappings ClassMappings
		{
			get
			{
				return (Framework.Plugin.IPluginClassMappings)EamonRT.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static Framework.Plugin.IPluginGlobals Globals
		{
			get
			{
				return (Framework.Plugin.IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}
	}
}
EamonCsCodeTemplate */
