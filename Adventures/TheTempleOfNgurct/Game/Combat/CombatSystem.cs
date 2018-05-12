
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;

namespace TheTempleOfNgurct.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void ExecuteAttack()
		{
			// Only use Fractional strength for regular attacks

			if (!BlastSpell)
			{
				UseFractionalStrength = true;
			}

			base.ExecuteAttack();
		}
	}
}
