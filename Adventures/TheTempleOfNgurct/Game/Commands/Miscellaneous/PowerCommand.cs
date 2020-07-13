
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public virtual void PrintAirCracklesWithEnergy()
		{
			gOut.Print("The air crackles with magical energy!");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				var rl = 0L;

				var monsters = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Uid != 53 && m.Friendliness < Friendliness.Friend && m.Seen && m.IsInRoom(ActorRoom));

				foreach (var m in monsters)
				{
					rl = gEngine.RollDice(1, 100, 0);

					if (rl > 50)
					{
						gOut.Print("{0} vanishes!", m.GetTheName(true));

						m.SetInLimbo();

						if (m.Uid == 30)
						{
							gGameState.KeyRingRoomUid = ActorRoom.Uid;
						}

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				rl = gEngine.RollDice(1, 100, 0);

				// Earthquake!

				if (rl < 26 && gGameState.Ro != 58)
				{
					gEngine.PrintEffectDesc(17);

					if (rl < 11)
					{
						gEngine.PrintEffectDesc(18);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}

					monsters = gEngine.GetRandomMonsterList(1, m => !m.IsCharacterMonster() && m.Seen && m.IsInRoom(ActorRoom));

					Debug.Assert(monsters != null);

					foreach (var m in monsters)
					{
						gOut.Print("{0} falls into the crack!", m.GetTheName(true));

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
					gEngine.PrintEffectDesc(15);

					gEngine.PrintEffectDesc(16);

					var room = gEngine.RollDice(1, 27, 32);

					gGameState.R2 = room;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.MoveMonsters = false;
					});

					GotoCleanup = true;

					goto Cleanup;
				}

				var heroMonster = gMDB[57];

				Debug.Assert(heroMonster != null);

				// The Hero appears

				if (rl < 71 && gGameState.Ro != 58 && !heroMonster.Seen)
				{
					PrintAirCracklesWithEnergy();

					heroMonster.SetInRoom(ActorRoom);

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// The Hero disappears

				if (rl < 71 && heroMonster.IsInRoom(ActorRoom))
				{
					gOut.Print("The Hero vanishes!  (The gods giveth...)");

					heroMonster.SetInLimbo();

					GotoCleanup = true;

					goto Cleanup;
				}

				// Gets wandering monster

				if (rl < 81 && gGameState.Ro != 58)
				{
					PrintAirCracklesWithEnergy();

					gEngine.GetWanderingMonster();

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// The lost room!

				if (rl < 86 && gGameState.Ro != 58)
				{
					PrintAirCracklesWithEnergy();

					gGameState.Ro = 58;

					gGameState.R2 = gGameState.Ro;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.MoveMonsters = false;
					});

					GotoCleanup = true;

					goto Cleanup;
				}

				gOut.Print("All your wounds are healed!");

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
