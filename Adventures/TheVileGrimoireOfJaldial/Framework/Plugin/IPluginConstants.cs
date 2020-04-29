
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Framework.Plugin
{
	public interface IPluginConstants : EamonRT.Framework.Plugin.IPluginConstants
	{
		long[] NonEmotingMonsterUids { get; set; }
	}
}
