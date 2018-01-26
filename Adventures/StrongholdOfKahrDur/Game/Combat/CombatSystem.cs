
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.Combat;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Combat
{
	[ClassMappings(typeof(EamonRT.Framework.Combat.ICombatSystem))]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		protected override void PrintCriticalHit()
		{
			Globals.Out.Write("Well struck!");
		}

		protected override void PrintBlowTurned()
		{
			if (DfMonster.Armor > 0)
			{
				var armorDesc = DfMonster.GetArmorDescString();

				Globals.Out.Write("{0}{1}Blow glances off {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", armorDesc);
			}
			else
			{
				base.PrintBlowTurned();
			}
		}

		protected override void BeginAttack()
		{
			// Necromancer (22) is impervious to weapon attacks

			if (DfMonster != null && DfMonster.Uid == 22)
			{
				OmitSkillGains = true;
			}

			base.BeginAttack();
		}

		protected override void AttackHit()
		{
			// Necromancer (22) is impervious to weapon attacks

			if (DfMonster.Uid == 22)
			{
				var rl = Globals.Engine.RollDice01(1, 4, 60);

				Globals.Engine.PrintEffectDesc(rl, false);

				CombatState = RTEnums.CombatState.EndAttack;
			}
			else
			{
				base.AttackHit();
			}
		}

		public override void ExecuteAttack()
		{
			if (BlastSpell)
			{
				var artifact = Globals.ADB[25];

				Debug.Assert(artifact != null);

				PrintBlast();

				// If player is wearing Wizard's Helm (25), blast spell is more potent

				ExecuteCalculateDamage(2, artifact.IsWornByCharacter() ? 12 : 5);
			}
			else
			{
				base.ExecuteAttack();
			}
		}
	}
}
