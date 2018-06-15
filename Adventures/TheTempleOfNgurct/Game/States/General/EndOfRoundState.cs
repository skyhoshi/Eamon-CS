
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeAfterRoundEnd)
			{
				var characterMonster = Globals.MDB[gameState.Cm];

				Debug.Assert(characterMonster != null);

				var ringArtifact = Globals.ADB[64];

				Debug.Assert(ringArtifact != null);

				// Ring of regeneration

				if (ringArtifact.IsWornByCharacter() && characterMonster.DmgTaken > 0 && ++gameState.Regenerate == 5)
				{
					Globals.GameState.ModDTTL(characterMonster.Friendliness, -1);

					characterMonster.DmgTaken--;

					gameState.Regenerate = 0;
				}

				// Bring in wandering monsters

				var rl = Globals.Engine.RollDice01(1, 100, 0);

				if (rl <= 4 && gameState.Ro != 58)        // rl <= 7
				{
					// Monsters won't wander into a locked cell

					var cellDoorArtifact = Globals.ADB[gameState.Ro == 45 ? 87 : gameState.Ro == 46 ? 88 : gameState.Ro == 55 ? 86 : 0];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate) : null;

					if (ac == null || ac.GetKeyUid() <= 0)
					{
						Globals.Engine.GetWanderingMonster();
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}

