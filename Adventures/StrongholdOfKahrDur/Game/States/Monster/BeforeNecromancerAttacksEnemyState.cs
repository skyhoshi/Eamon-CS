
// BeforeNecromancerAttacksEnemyState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using StrongholdOfKahrDur.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class BeforeNecromancerAttacksEnemyState : EamonRT.Game.States.State, IBeforeNecromancerAttacksEnemyState
	{
		protected virtual Eamon.Framework.IRoom Room { get; set; }

		protected virtual void SummonMonster(long monsterUid)
		{
			// Necromancer summons other monsters...

			var monster = Globals.MDB[monsterUid];

			Debug.Assert(monster != null);

			// Dead, hasn't been previously summoned, or is somewhere else

			if (monster.IsInLimbo() || !monster.IsInRoom(Room))
			{
				// Preset 'Seen' flag for smoother effect

				monster.Seen = true;

				// Put monster in room

				monster.SetInRoom(Room);

				// Reset group size to one

				monster.GroupCount = 1;

				monster.InitGroupCount = 1;

				monster.OrigGroupCount = 1;

				// Reset to 0 damage taken

				monster.DmgTaken = 0;
			}
			else
			{
				// Monster is already summoned and present so add another to the group

				monster.GroupCount++;

				monster.InitGroupCount++;

				monster.OrigGroupCount++;
			}

			Globals.Engine.CheckEnemies();
		}

		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			var monster01 = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(monster01 != null);

			Room = monster01.GetInRoom();

			Debug.Assert(Room != null);

			if (!monster.IsInRoom(Room))
			{
				goto Cleanup;
			}

			var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.DfMonster = monster01;

				x.OmitArmor = true;
			});

			var rl = Globals.Engine.RollDice01(1, 7, 0);

			Globals.Engine.PrintEffectDesc(69 + rl);

			switch(rl)
			{
				// Magical energy drain

				case 1:

					var spellValues = EnumUtil.GetValues<Enums.Spell>();

					foreach (var sv in spellValues)
					{
						Globals.GameState.SetSa(sv, (long)(Globals.GameState.GetSa(sv) * 0.8));

						if (Globals.GameState.GetSa(sv) < 5)
						{
							Globals.GameState.SetSa(sv, 5);
						}
					}

					break;

				// Lightning Spell

				case 2:

					combatSystem.ExecuteCalculateDamage(1, 8);

					break;

				// Fireball Spell

				case 3:

					combatSystem.ExecuteCalculateDamage(1, 6);

					break;

				// Necrotic Spell

				case 4:

					combatSystem.ExecuteCalculateDamage(1, 10);

					break;

				// Summon fire demon (25)

				case 5:

					SummonMonster(25);

					break;

				// Summon hell hound (24)

				case 6:

					SummonMonster(24);

					break;

				// Summon demonic serpent (23)

				case 7:

					SummonMonster(23);

					break;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public BeforeNecromancerAttacksEnemyState()
		{
			Name = "BeforeNecromancerAttacksEnemyState";
		}
	}
}
