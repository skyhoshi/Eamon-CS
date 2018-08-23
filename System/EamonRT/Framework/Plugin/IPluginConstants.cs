
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Plugin
{
	public interface IPluginConstants : EamonDD.Framework.Plugin.IPluginConstants
	{
		long StartRoom { get; }

		long NumSaveSlots { get; }

		long ScaledHardinessUnarmedMaxDamage { get; }

		double ScaledHardinessMaxDamageDivisor { get; }

		string CommandPrompt { get; }

		string PageSep { get; }

		string RtProgVersion { get; }
	}
}

/* EamonCsCodeTemplate

// IPluginConstants.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginConstants : EamonRT.Framework.Plugin.IPluginConstants
	{
		
	}
}
EamonCsCodeTemplate */
