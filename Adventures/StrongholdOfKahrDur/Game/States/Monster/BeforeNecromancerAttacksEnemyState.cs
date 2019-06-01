
// BeforeNecromancerAttacksEnemyState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class BeforeNecromancerAttacksEnemyState : EamonRT.Game.States.State, Framework.States.IBeforeNecromancerAttacksEnemyState
	{
		public override void Execute()
		{
			var necromancerMonster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(necromancerMonster != null);

			var characterMonster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(characterMonster != null);

			var room = characterMonster.GetInRoom();

			Debug.Assert(room != null);

			if (!necromancerMonster.IsInRoom(room))
			{
				goto Cleanup;
			}

			var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.DfMonster = characterMonster;

				x.OmitArmor = true;
			});

			var rl = Globals.Engine.RollDice(1, 7, 0);

			Globals.Engine.PrintEffectDesc(69 + rl);

			switch(rl)
			{
				// Magical energy drain

				case 1:

					var spellValues = EnumUtil.GetValues<Spell>();

					foreach (var sv in spellValues)
					{
						if (Globals.GameState.GetSa(sv) > 5)
						{
							Globals.GameState.SetSa(sv, (long)(Globals.GameState.GetSa(sv) * 0.8));

							if (Globals.GameState.GetSa(sv) < 5)
							{
								Globals.GameState.SetSa(sv, 5);
							}
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

					Globals.Engine.SummonMonster(room, 25);

					break;

				// Summon hell hound (24)

				case 6:

					Globals.Engine.SummonMonster(room, 24);

					break;

				// Summon demonic serpent (23)

				case 7:

					Globals.Engine.SummonMonster(room, 23);

					break;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public BeforeNecromancerAttacksEnemyState()
		{
			Name = "BeforeNecromancerAttacksEnemyState";
		}
	}
}
