
// PowerCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheTempleOfNgurct.Framework;
using TheTempleOfNgurct.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPowerCommand))]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected virtual void PrintAirCracklesWithEnergy()
		{
			Globals.Out.Write("{0}The air crackles with magical energy!{0}", Environment.NewLine);
		}

		protected override void PlayerProcessEvents()
		{
			var rl = 0L;

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var monsters = Globals.Engine.GetMonsterList(() => true, m => !m.IsCharacterMonster() && m.Uid != 53 && m.Friendliness < Enums.Friendliness.Friend && m.Seen && m.IsInRoom(ActorRoom));

			foreach (var m in monsters)
			{
				rl = Globals.Engine.RollDice01(1, 100, 0);

				if (rl > 50)
				{
					Globals.Out.WriteLine("{0}{1} vanishes!", Environment.NewLine, m.GetDecoratedName03(true, true, false, false, Globals.Buf));

					m.SetInLimbo();

					if (m.Uid == 30)
					{
						gameState.KeyRingRoomUid = ActorRoom.Uid;
					}

					Globals.RtEngine.CheckEnemies();

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

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}

				monsters = Globals.RtEngine.GetRandomMonsterList(1, m => !m.IsCharacterMonster() && m.Seen && m.IsInRoom(ActorRoom));

				Debug.Assert(monsters != null);

				foreach (var m in monsters)
				{
					Globals.Out.WriteLine("{0}{1} falls into the crack!", Environment.NewLine, m.GetDecoratedName03(true, true, false, false, Globals.Buf));

					var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
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

				Globals.RtEngine.CheckEnemies();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

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

				Globals.RtEngine.CheckEnemies();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;

				goto Cleanup;
			}

			// The Hero disappears

			if (rl < 71 && heroMonster.IsInRoom(ActorRoom))
			{
				Globals.Out.Write("{0}The Hero vanishes!  (The gods giveth...){0}", Environment.NewLine);

				heroMonster.SetInLimbo();

				Globals.RtEngine.CheckEnemies();

				GotoCleanup = true;

				goto Cleanup;
			}

			// Gets wandering monster

			if (rl < 81 && gameState.Ro != 58)
			{
				PrintAirCracklesWithEnergy();

				Globals.RtEngine.CastTo<IRtEngine>().GetWanderingMonster();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;

				goto Cleanup;
			}

			// The lost room!

			if (rl < 86 && gameState.Ro != 58)
			{
				PrintAirCracklesWithEnergy();

				ActorMonster.SetInRoomUid(58);

				Globals.RtEngine.CheckEnemies();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;

				goto Cleanup;
			}

			Globals.Out.Write("{0}All your wounds are healed!{0}", Environment.NewLine);

			Globals.GameState.ModDTTL(ActorMonster.Friendliness, -ActorMonster.DmgTaken);

			ActorMonster.DmgTaken = 0;

		Cleanup:

			;
		}
	}
}
