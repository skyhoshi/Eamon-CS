
// AttackCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheTempleOfNgurct.Framework;
using TheTempleOfNgurct.Framework.Commands;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IAttackCommand))]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			if (BlastSpell || ActorMonster.Weapon > 0)
			{
				// ATTACK/BLAST statue

				if (DobjArtifact != null && DobjArtifact.Uid == 50)
				{
					Globals.RtEngine.PrintMonsterAlive(DobjArtifact);

					DobjArtifact.SetInLimbo();

					var ngurctMonster = Globals.MDB[53];

					Debug.Assert(ngurctMonster != null);

					ngurctMonster.SetInRoom(ActorRoom);

					Globals.RtEngine.CheckEnemies();

					var command = Globals.CreateInstance<EamonRT.Framework.Commands.IAttackCommand>(x =>
					{
						x.BlastSpell = BlastSpell;

						x.CheckAttack = CheckAttack;
					});

					CopyCommandData(command);

					command.DobjArtifact = null;

					command.DobjMonster = ngurctMonster;

					NextState = command;
				}

				// Fireball wand

				else if (DobjMonster != null && ActorMonster.Weapon == 63)
				{
					Globals.Out.Write("{0}What is the trigger word? ", Environment.NewLine);

					Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

					if (!string.Equals(Globals.Buf.ToString(), "fire", StringComparison.OrdinalIgnoreCase))
					{
						Globals.Out.WriteLine("{0}Wrong!  Nothing happens!", Environment.NewLine);

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

						goto Cleanup;
					}

					if (gameState.WandCharges <= 0)
					{
						Globals.Out.WriteLine("{0}The fireball wand is exhausted!", Environment.NewLine);

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

						goto Cleanup;
					}

					gameState.WandCharges--;

					Globals.Out.WriteLine("{0}The {1} is filled with an incandescent fireball!", Environment.NewLine, ActorRoom.EvalRoomType("room", "area"));

					var monsters = Globals.RtEngine.GetRandomMonsterList(10, m => !m.IsCharacterMonster() && m.Uid != DobjMonster.Uid && m.Seen && m.IsInRoom(ActorRoom));

					Debug.Assert(monsters != null);

					if (monsters.Count > 0)
					{
						monsters.Insert(0, DobjMonster);
					}
					else
					{
						monsters.Add(DobjMonster);
					}

					Globals.FireDamage = true;

					foreach (var m in monsters)
					{
						var rl = Globals.Engine.RollDice01(1, 100, 0);

						var savedVsFire = (m.Hardiness / 4) > 4 && rl < 51;

						Globals.RtEngine.MonsterGetsAggravated(m);

						var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.DfMonster = m;

							x.OmitArmor = true;
						});

						combatSystem.ExecuteCalculateDamage(savedVsFire ? 3 : 6, 6);
					}

					Globals.FireDamage = false;

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

					goto Cleanup;
				}
				else
				{
					base.PlayerExecute();
				}
			}
			else
			{
				base.PlayerExecute();
			}

		Cleanup:

			;
		}
	}
}
