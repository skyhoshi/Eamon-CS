
// AttackLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AttackLoopIncrementState : State, IAttackLoopIncrementState
	{
		public override void Execute()
		{
			var monster = gMDB[Globals.LoopMonsterUid];

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

			if (monster.ShouldReadyWeapon() && Globals.LoopMemberNumber == 1 && Globals.LoopAttackNumber == 1 && monster.Weapon <= 0)
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
