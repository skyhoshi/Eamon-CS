
// AttackLoopIncrementState.cs

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
	public class AttackLoopIncrementState : State, IAttackLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Globals.LoopAttackNumber++;

			if (LoopMonster.GroupCount < Globals.LoopGroupCount)
			{
				Globals.LoopMemberNumber--;
			}

			if (LoopMonster.IsInLimbo() || LoopMonster.GroupCount < Globals.LoopGroupCount || Globals.LoopAttackNumber > Math.Abs(LoopMonster.AttackCount) || Globals.LoopAttackNumber > 25)
			{
				NextState = Globals.CreateInstance<IMemberLoopIncrementState>();

				goto Cleanup;
			}

			if (LoopMonster.ShouldReadyWeapon() && Globals.LoopMemberNumber == 1 && Globals.LoopAttackNumber == 1 && LoopMonster.Weapon <= 0)
			{
				NextState = Globals.CreateInstance<IWeaponArtifactLoopInitializeState>();

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
