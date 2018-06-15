
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected virtual void PrintAirCracklesWithEnergy()
		{
			Globals.Out.Print("The air crackles with magical energy!");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				var rl = 0L;

				var monsters = Globals.Engine.GetMonsterList(() => true, m => !m.IsCharacterMonster() && m.Uid != 53 && m.Friendliness < Enums.Friendliness.Friend && m.Seen && m.IsInRoom(ActorRoom));

				foreach (var m in monsters)
				{
					rl = Globals.Engine.RollDice01(1, 100, 0);

					if (rl > 50)
					{
						Globals.Out.Print("{0} vanishes!", m.GetDecoratedName03(true, true, false, false, Globals.Buf));

						m.SetInLimbo();

						if (m.Uid == 30)
						{
							gameState.KeyRingRoomUid = ActorRoom.Uid;
						}

						Globals.Engine.CheckEnemies();

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				rl = Globals.Engine.RollDice01(1, 100, 0);

				// Earthquake!

				if (rl < 26 && gameState.Ro != 58)
				{
					Globals.Engine.PrintEffectDesc(17);

					if (rl < 11)
					{
						Globals.Engine.PrintEffectDesc(18);

						gameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}

					monsters = Globals.Engine.GetRandomMonsterList(1, m => !m.IsCharacterMonster() && m.Seen && m.IsInRoom(ActorRoom));

					Debug.Assert(monsters != null);

					foreach (var m in monsters)
					{
						Globals.Out.Print("{0} falls into the crack!", m.GetDecoratedName03(true, true, false, false, Globals.Buf));

						var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.DfMonster = m;

							x.OmitArmor = false;
						});

						combatSystem.ExecuteCalculateDamage(1, 100);
					}

					GotoCleanup = true;

					goto Cleanup;
				}

				// Annoy higher power

				if (rl < 51)
				{
					Globals.Engine.PrintEffectDesc(15);

					Globals.Engine.PrintEffectDesc(16);

					var room = Globals.Engine.RollDice01(1, 27, 32);

					ActorMonster.SetInRoomUid(room);

					Globals.Engine.CheckEnemies();

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				var heroMonster = Globals.MDB[57];

				Debug.Assert(heroMonster != null);

				// The Hero appears

				if (rl < 71 && gameState.Ro != 58 && !heroMonster.Seen)
				{
					PrintAirCracklesWithEnergy();

					heroMonster.SetInRoom(ActorRoom);

					Globals.Engine.CheckEnemies();

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// The Hero disappears

				if (rl < 71 && heroMonster.IsInRoom(ActorRoom))
				{
					Globals.Out.Print("The Hero vanishes!  (The gods giveth...)");

					heroMonster.SetInLimbo();

					Globals.Engine.CheckEnemies();

					GotoCleanup = true;

					goto Cleanup;
				}

				// Gets wandering monster

				if (rl < 81 && gameState.Ro != 58)
				{
					PrintAirCracklesWithEnergy();

					Globals.Engine.GetWanderingMonster();

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// The lost room!

				if (rl < 86 && gameState.Ro != 58)
				{
					PrintAirCracklesWithEnergy();

					ActorMonster.SetInRoomUid(58);

					Globals.Engine.CheckEnemies();

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				Globals.Out.Print("All your wounds are healed!");

				Globals.GameState.ModDTTL(ActorMonster.Friendliness, -ActorMonster.DmgTaken);

				ActorMonster.DmgTaken = 0;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}

		Cleanup:

			;
		}
	}
}
