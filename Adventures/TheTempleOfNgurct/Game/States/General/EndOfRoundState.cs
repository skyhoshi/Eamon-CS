﻿
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterRoundEnd)
			{
				var characterMonster = gMDB[gGameState.Cm];

				Debug.Assert(characterMonster != null);

				var ringArtifact = gADB[64];

				Debug.Assert(ringArtifact != null);

				// Ring of regeneration

				if (ringArtifact.IsWornByCharacter() && characterMonster.DmgTaken > 0 && ++gGameState.Regenerate == 5)
				{
					characterMonster.DmgTaken--;

					gGameState.Regenerate = 0;
				}

				// Bring in wandering monsters

				var rl = gEngine.RollDice(1, 100, 0);

				if (rl <= 4 && gGameState.Ro != 58)        // rl <= 7
				{
					// Monsters won't wander into a locked cell

					var cellDoorArtifact = gADB[gGameState.Ro == 45 ? 87 : gGameState.Ro == 46 ? 88 : gGameState.Ro == 55 ? 86 : 0];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.DoorGate : null;

					if (ac == null || ac.GetKeyUid() <= 0)
					{
						gEngine.GetWanderingMonster();
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}

