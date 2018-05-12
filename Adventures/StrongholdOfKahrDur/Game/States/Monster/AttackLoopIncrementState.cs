
// AttackLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class AttackLoopIncrementState : EamonRT.Game.States.AttackLoopIncrementState, IAttackLoopIncrementState
	{
		protected override bool ShouldMonsterRearm(IMonster monster)
		{
			Debug.Assert(monster != null);

			// Necromancer never tries to pick up or ready weapon

			return monster.Uid != 22 ? base.ShouldMonsterRearm(monster) : false;
		}
	}
}
