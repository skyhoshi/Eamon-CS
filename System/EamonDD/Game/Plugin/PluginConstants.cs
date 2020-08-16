
// PluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using EamonDD.Framework.Plugin;

namespace EamonDD.Game.Plugin
{
	public class PluginConstants : Eamon.Game.Plugin.PluginConstants, IPluginConstants
	{
		public virtual string DevenvExePath { get; protected set; }

		public virtual string DdProgVersion { get; protected set; }

		public PluginConstants()
		{
			DevenvExePath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe";

			DdProgVersion = ProgVersion;
		}
	}
}
