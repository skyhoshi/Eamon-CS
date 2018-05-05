
// AttackLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class AttackLoopIncrementState : EamonRT.Game.States.AttackLoopIncrementState, EamonRT.Framework.States.IAttackLoopIncrementState
	{
		protected override bool ShouldMonsterRearm(Eamon.Framework.IMonster monster)
		{
			Debug.Assert(monster != null);

			// Necromancer never tries to pick up or ready weapon

			return monster.Uid != 22 ? base.ShouldMonsterRearm(monster) : false;
		}
	}
}
