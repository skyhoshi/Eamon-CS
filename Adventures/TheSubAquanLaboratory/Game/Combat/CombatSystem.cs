
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		protected override void SetAttackDesc()
		{
			AttackDesc = "attack{0}";

			if (!UseAttacks)
			{
				if (OfMonster.IsCharacterMonster())
				{
					AttackDesc = OfMonster.GetAttackDescString(OfWeapon);
				}
				else if (OfMonster.IsInRoomLit())
				{
					if (OfMonster.Uid > 19 && OfMonster.Uid < 23)
					{
						AttackDesc = "zap{0}";
					}
					else if (OfMonster.CombatCode != CombatCode.Attacks && (!((Framework.IMonster)OfMonster).IsAndroid() || OfMonster.Weapon > 0))
					{
						AttackDesc = OfMonster.GetAttackDescString(OfWeapon);
					}
				}
			}
		}

		protected override void PrintBlowTurned()
		{
			if (DfMonster.Uid == Globals.GameState.Cm && DfMonster.Armor < 1)
			{
				Globals.Out.Write("{0}{1}Blow turned!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
			}
			else
			{
				var armorDesc = DfMonster.GetArmorDescString();

				Globals.Out.Write("{0}{1}Blow bounces off {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", armorDesc);
			}
		}
	}
}
