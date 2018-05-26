
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		protected override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeAfterRoundEnd)
			{
				var room = Globals.RDB[Globals.GameState.Ro];

				Debug.Assert(room != null);

				// Energy mace fizzles

				if (gameState.EnergyMaceCharge > 0)
				{
					var energyMaceArtifact = Globals.ADB[80];

					Debug.Assert(energyMaceArtifact != null);

					var monster = energyMaceArtifact.GetCarriedByMonster();

					if (energyMaceArtifact.IsInRoom(room) || energyMaceArtifact.IsCarriedByCharacter() || (monster != null && monster.IsInRoom(room)))
					{
						if (--gameState.EnergyMaceCharge == 0)
						{
							var ac = energyMaceArtifact.GetCategories(0);

							Debug.Assert(ac != null);

							ac.Field1 = 0;

							ac.Field3 = 1;

							ac.Field4 = 6;

							energyMaceArtifact.Value = 15;

							Globals.Engine.PrintEffectDesc(31);
						}
					}
				}

				// Laser scalpel fizzles

				if (gameState.LaserScalpelCharge > 0)
				{
					var scalpelArtifact = Globals.ADB[76];

					Debug.Assert(scalpelArtifact != null);

					var monster = scalpelArtifact.GetCarriedByMonster();

					if (scalpelArtifact.IsInRoom(room) || scalpelArtifact.IsCarriedByCharacter() || (monster != null && monster.IsInRoom(room)))
					{
						if (--gameState.LaserScalpelCharge == 0)
						{
							scalpelArtifact.Value = 15;

							Globals.Engine.ConvertWeaponToGoldOrTreasure(scalpelArtifact, false);

							Globals.Engine.PrintEffectDesc(32);
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}

