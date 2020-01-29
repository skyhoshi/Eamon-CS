
// MonsterReadiedWeaponCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterReadiedWeaponCheckState : State, IMonsterReadiedWeaponCheckState
	{
		public override void Execute()
		{
			var monster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			if (monster.Weapon > 0)
			{
				Globals.LoopAttackNumber = Math.Abs(monster.AttackCount);

				NextState = Globals.CreateInstance<IAttackLoopIncrementState>();
			
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBeforeMonsterAttacksEnemyState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterReadiedWeaponCheckState()
		{
			Name = "MonsterReadiedWeaponCheckState";
		}
	}
}
