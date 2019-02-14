
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace StrongholdOfKahrDur.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		new IEngine Engine { get; set; }
	}
}
