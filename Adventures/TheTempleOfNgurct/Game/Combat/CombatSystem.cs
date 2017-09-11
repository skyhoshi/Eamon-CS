
// CombatSystem.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Combat;
using Enums = Eamon.Framework.Primitive.Enums;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Combat
{
	[ClassMappings(typeof(EamonRT.Framework.Combat.ICombatSystem))]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public virtual bool UseFractionalStrength { get; set; }

		protected override void CalculateDamage()
		{
			Debug.Assert(OfMonster != null || !UseFractionalStrength);

			Debug.Assert(DfMonster != null);

			Debug.Assert(D > 0 && S > 0 && A >= 0 && A <= 1);

			if (UseFractionalStrength)
			{
				_d2 = 0;

				var xx = (double)(OfMonster.Hardiness - OfMonster.DmgTaken) / (double)OfMonster.Hardiness;      // Fractional strength

				var yy = xx < .5 ? .5 + (xx * (.083 + (.833 * xx))) : (-.75) + (xx * (4.25 - (2.5 * xx)));

				if (yy > 1)
				{
					yy = 1;
				}

				for (var i = 0; i < D; i++)
				{
					_d2 += (long)Math.Round(yy * (MaxDamage ? S : Globals.Engine.RollDice01(1, S, 0)));
				}
			}
			else
			{
				_d2 = MaxDamage ? (D * S) + M : Globals.Engine.RollDice01(D, S, M);
			}

			_d2 -= (A * DfMonster.Armor);

			if (DfMonster.IsCharacterMonster())
			{
				DfArmor = Globals.GameState.Ar > 0 ? Globals.ADB[Globals.GameState.Ar] : null;

				ArAc = DfArmor != null ? DfArmor.GetArtifactClass(Enums.ArtifactType.Wearable) : null;

				if (ArAc != null && (ArAc.Field5 / 2) > 2)
				{
					_d2 -= (A * 2);
				}
			}

			CombatState = RTEnums.CombatState.CheckArmor;
		}

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
