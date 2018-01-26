
// CombatState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Primitive.Enums
{
	public enum CombatState : long
	{
		None = 0,
		BeginAttack,
		AttackMiss,
		AttackFumble,
		AttackHit,
		CalculateDamage,
		CheckArmor,
		CheckMonsterStatus,
		EndAttack,
		User1,
		User2,
		User3
	}
}
