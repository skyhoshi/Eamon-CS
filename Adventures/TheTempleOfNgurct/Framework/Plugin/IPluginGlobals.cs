
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace TheTempleOfNgurct.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		new IEngine Engine { get; set; }

		bool FireDamage { get; set; }
	}
}
