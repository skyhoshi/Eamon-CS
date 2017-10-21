
// IPluginGlobals.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace StrongholdOfKahrDur.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		new IEngine Engine { get; set; }
	}
}
