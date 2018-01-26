
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using BeginnersForest.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IBurnDownSpeedSpellState))]
	public class BurnDownSpeedSpellState : EamonRT.Game.States.BurnDownSpeedSpellState, IBurnDownSpeedSpellState
	{
		protected override void PrintSpeedSpellExpired()
		{
			base.PrintSpeedSpellExpired();

			// Super Magic Agility Ring (patent pending)

			var artifact = Globals.ADB[2];

			Debug.Assert(artifact != null);

			if (artifact.IsWornByCharacter())
			{
				Globals.Engine.PrintEffectDesc(17);
			}

			artifact.SetInLimbo();
		}
	}
}
