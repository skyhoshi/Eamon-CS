
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		bool EncounterSurprises { get; set; }

		/// <summary></summary>
		bool CarrionCrawlerFlails { get; set; }

		/// <summary></summary>
		long EventRoll { get; set; }

		/// <summary></summary>
		long ScaleRoll { get; set; }

		/// <summary></summary>
		long InitiativeMonsterUid { get; set; }
	}
}
