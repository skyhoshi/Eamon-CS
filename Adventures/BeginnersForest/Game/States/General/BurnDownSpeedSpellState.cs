
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : EamonRT.Game.States.BurnDownSpeedSpellState, IBurnDownSpeedSpellState
	{
		protected override void PrintSpeedSpellExpired()
		{
			base.PrintSpeedSpellExpired();

			// Super Magic Agility Ring (patent pending)

			var ringArtifact = Globals.ADB[2];

			Debug.Assert(ringArtifact != null);

			if (ringArtifact.IsWornByCharacter())
			{
				Globals.Engine.PrintEffectDesc(17);
			}

			ringArtifact.SetInLimbo();
		}
	}
}
