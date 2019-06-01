
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
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
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
