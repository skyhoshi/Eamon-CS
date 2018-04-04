
// AttackLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AttackLoopIncrementState : State, IAttackLoopIncrementState
	{
		protected virtual bool ShouldMonsterRearm(IMonster monster)
		{
			return true;
		}

		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			Globals.LoopAttackNumber++;

			if (monster.GroupCount < Globals.LoopGroupCount)
			{
				Globals.LoopMemberNumber--;
			}

			if (monster.IsInLimbo() || monster.GroupCount < Globals.LoopGroupCount || Globals.LoopAttackNumber > Math.Abs(monster.AttackCount) || Globals.LoopAttackNumber > 25)
			{
				NextState = Globals.CreateInstance<IMemberLoopIncrementState>();

				goto Cleanup;
			}

			if (ShouldMonsterRearm(monster) && Globals.LoopMemberNumber == 1 && Globals.LoopAttackNumber == 1 && monster.Weapon <= 0)
			{
				NextState = Globals.CreateInstance<IArtifactLoopInitializeState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBeforeMonsterAttacksEnemyState>();
			}

			Globals.NextState = NextState;
		}

		public AttackLoopIncrementState()
		{
			Name = "AttackLoopIncrementState";
		}
	}
}
