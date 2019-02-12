
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonDD.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginConstants : Eamon.Framework.Plugin.IPluginConstants
	{
		/// <summary></summary>
		string DevenvExePath { get; }

		/// <summary></summary>
		string DdProgVersion { get; }
	}
}
