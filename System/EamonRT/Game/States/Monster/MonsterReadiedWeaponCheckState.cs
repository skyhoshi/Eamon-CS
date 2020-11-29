
// MonsterReadiedWeaponCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterReadiedWeaponCheckState : State, IMonsterReadiedWeaponCheckState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			if (LoopMonster.Weapon > 0)
			{
				Globals.LoopAttackNumber = Math.Abs(LoopMonster.AttackCount);

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
			Uid = 20;

			Name = "MonsterReadiedWeaponCheckState";
		}
	}
}
