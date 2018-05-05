
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, EamonRT.Framework.States.IEndOfRoundState
	{
		protected override void ProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(room != null);

			// Energy mace fizzles

			if (gameState.EnergyMaceCharge > 0)
			{
				var artifact = Globals.ADB[80];

				Debug.Assert(artifact != null);

				var monster = artifact.GetCarriedByMonster();

				if (artifact.IsInRoom(room) || artifact.IsCarriedByCharacter() || (monster != null && monster.IsInRoom(room)))
				{
					if (--gameState.EnergyMaceCharge == 0)
					{
						var ac = artifact.GetCategories(0);

						Debug.Assert(ac != null);

						ac.Field1 = 0;

						ac.Field3 = 1;

						ac.Field4 = 6;

						artifact.Value = 15;

						Globals.Engine.PrintEffectDesc(31);
					}
				}
			}

			// Laser scalpel fizzles

			if (gameState.LaserScalpelCharge > 0)
			{
				var artifact = Globals.ADB[76];

				Debug.Assert(artifact != null);

				var monster = artifact.GetCarriedByMonster();

				if (artifact.IsInRoom(room) || artifact.IsCarriedByCharacter() || (monster != null && monster.IsInRoom(room)))
				{
					if (--gameState.LaserScalpelCharge == 0)
					{
						artifact.Value = 15;

						Globals.Engine.ConvertWeaponToGoldOrTreasure(artifact, false);

						Globals.Engine.PrintEffectDesc(32);
					}
				}
			}

			base.ProcessEvents();
		}
	}
}

