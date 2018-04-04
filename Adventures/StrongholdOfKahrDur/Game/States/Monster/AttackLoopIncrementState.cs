
// AttackLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.States;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IAttackLoopIncrementState))]
	public class AttackLoopIncrementState : EamonRT.Game.States.AttackLoopIncrementState, IAttackLoopIncrementState
	{
		protected override bool ShouldMonsterRearm(Eamon.Framework.IMonster monster)
		{
			Debug.Assert(monster != null);

			// Necromancer never tries to pick up or ready weapon

			return monster.Uid != 22 ? base.ShouldMonsterRearm(monster) : false;
		}
	}
}
