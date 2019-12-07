
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeAfterExtinguishLightSourceCheck)
			{
				var rl = Globals.Engine.RollDice(1, 100, 0);

				// Spear trap

				if (gameState.Ro == 5 && rl < 20)
				{
					Globals.Engine.PrintEffectDesc(19);

					var monsters = Globals.Engine.GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						Globals.Engine.ApplyTrapDamage(s => NextState = s, m, 1, 6, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Loose rocks

				if (gameState.Ro == 11 && rl < 33)
				{
					Globals.Engine.PrintEffectDesc(20);

					var monsters = Globals.Engine.GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						Globals.Engine.ApplyTrapDamage(s => NextState = s, m, 1, 4, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Gas trap

				if (gameState.Ro == 16 && rl < 15)
				{
					Globals.Engine.PrintEffectDesc(21);

					var monsters = Globals.Engine.GetTrapMonsterList(3, gameState.Ro);

					foreach (var m in monsters)
					{
						Globals.Engine.ApplyTrapDamage(s => NextState = s, m, 2, 6, true);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Crossbow trap

				if (gameState.Ro == 32 && rl < 51)
				{
					Globals.Engine.PrintEffectDesc(22);

					var monsters = Globals.Engine.GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						Globals.Engine.ApplyTrapDamage(s => NextState = s, m, 1, 8, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Scything blade trap

				if (gameState.Ro == 57 && rl < 20)
				{
					Globals.Engine.PrintEffectDesc(23);

					var monsters = Globals.Engine.GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						Globals.Engine.ApplyTrapDamage(s => NextState = s, m, 2, 6, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Reveal secret doors

				var room1 = Globals.RDB[24];

				Debug.Assert(room1 != null);

				var secretDoorArtifact1 = Globals.ADB[83];

				Debug.Assert(secretDoorArtifact1 != null);

				if (gameState.Ro == 24 && gameState.R3 == 41 && secretDoorArtifact1.IsInLimbo())
				{
					secretDoorArtifact1.SetInRoomUid(24);

					secretDoorArtifact1.DoorGate.SetOpen(true);

					room1.SetDirectionDoor(Direction.North, secretDoorArtifact1);
				}

				var room2 = Globals.RDB[48];

				Debug.Assert(room2 != null);

				var secretDoorArtifact2 = Globals.ADB[84];

				Debug.Assert(secretDoorArtifact2 != null);

				if (gameState.Ro == 48 && gameState.R3 == 49 && secretDoorArtifact2.IsInLimbo())
				{
					secretDoorArtifact2.SetInRoomUid(48);

					secretDoorArtifact2.DoorGate.SetOpen(true);

					room2.SetDirectionDoor(Direction.South, secretDoorArtifact2);
				}
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
