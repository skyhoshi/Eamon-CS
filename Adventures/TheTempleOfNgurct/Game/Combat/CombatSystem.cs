
// CombatSystem.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Combat;

namespace TheTempleOfNgurct.Game.Combat
{
	[ClassMappings(typeof(EamonRT.Framework.Combat.ICombatSystem))]
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
