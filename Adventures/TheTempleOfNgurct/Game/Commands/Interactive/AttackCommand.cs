
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			if (BlastSpell || gActorMonster.Weapon > 0)
			{
				// ATTACK/BLAST statue

				if (gDobjArtifact != null && gDobjArtifact.Uid == 50)
				{
					gEngine.PrintMonsterAlive(gDobjArtifact);

					gDobjArtifact.SetInLimbo();

					var ngurctMonster = gMDB[53];

					Debug.Assert(ngurctMonster != null);

					ngurctMonster.SetInRoom(gActorRoom);

					var command = Globals.CreateInstance<IAttackCommand>(x =>
					{
						x.BlastSpell = BlastSpell;

						x.CheckAttack = CheckAttack;
					});

					CopyCommandData(command);

					command.Dobj = ngurctMonster;

					NextState = command;
				}

				// Fireball wand

				else if (!BlastSpell && gDobjMonster != null && gActorMonster.Weapon == 63)
				{
					gOut.Write("{0}What is the trigger word? ", Environment.NewLine);

					Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

					if (!string.Equals(Globals.Buf.ToString(), "fire", StringComparison.OrdinalIgnoreCase))
					{
						gOut.Print("Wrong!  Nothing happens!");

						NextState = Globals.CreateInstance<IMonsterStartState>();

						goto Cleanup;
					}

					if (gGameState.WandCharges <= 0)
					{
						gOut.Print("The fireball wand is exhausted!");

						NextState = Globals.CreateInstance<IMonsterStartState>();

						goto Cleanup;
					}

					gGameState.WandCharges--;

					gOut.Print("The {0} is filled with an incandescent fireball!", gActorRoom.EvalRoomType("room", "area"));

					var slaveGirlFireballCheck = false;

					var slaveGirlArtifact = gADB[81];

					Debug.Assert(slaveGirlArtifact != null);

					var slaveGirlMonster = gMDB[54];

					Debug.Assert(slaveGirlMonster != null);

					if (slaveGirlArtifact.IsInRoom(gActorRoom))
					{
						slaveGirlMonster.SetInRoom(gActorRoom);

						slaveGirlMonster.Seen = true;

						slaveGirlFireballCheck = true;
					}

					var monsters = gEngine.GetRandomMonsterList(9, m => !m.IsCharacterMonster() && m.Uid != gDobjMonster.Uid && m.Seen && m.IsInRoom(gActorRoom));

					Debug.Assert(monsters != null);

					if (monsters.Count > 0)
					{
						monsters.Insert(0, gDobjMonster);
					}
					else
					{
						monsters.Add(gDobjMonster);
					}

					Globals.FireDamage = true;

					foreach (var m in monsters)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						var savedVsFire = (m.Hardiness / 4) > 4 && rl < 51;

						gEngine.MonsterGetsAggravated(m);

						var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.DfMonster = m;

							x.OmitArmor = true;
						});

						combatSystem.ExecuteCalculateDamage(savedVsFire ? 3 : 6, 6);

						Globals.Thread.Sleep(gGameState.PauseCombatMs);
					}

					Globals.FireDamage = false;

					if (slaveGirlFireballCheck)
					{
						slaveGirlMonster.Seen = false;

						if (slaveGirlMonster.IsInLimbo())
						{
							slaveGirlArtifact.SetInLimbo();
						}
						else
						{
							slaveGirlMonster.SetInLimbo();
						}
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();

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
