
// PluginContext.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Plugin;

namespace Eamon.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants { get; set; }

		public static IPluginClassMappings ClassMappings { get; set; }

		public static IPluginGlobals Globals { get; set; }
	}
}
