﻿
// PluginConstants.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{
		public virtual long[] NonEmotingMonsterUids { get; set; }

		public PluginConstants()
		{
			// Accommodate oversized Effect Descs

			EffDescLen = 1024;

			// Restrict number of save game slots

			NumSaveSlots = 1;

			NonEmotingMonsterUids = new long[] { 13, 18, 19, 20, 22, 25, 31, 32, 38 };
		}
	}
}
