
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual bool EncounterSurprises { get; set; }

		public virtual bool CarrionCrawlerFlails { get; set; }

		public virtual long EventRoll { get; set; }

		public virtual long InitiativeMonsterUid { get; set; }
	}
}
