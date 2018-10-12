
// ICombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework;
using EamonRT.Framework.States;
using RTEnums = EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework.Combat
{
	public interface ICombatSystem
	{
		Action<IState> SetNextStateFunc { get; set; }

		IMonster OfMonster { get; set; }

		IMonster DfMonster { get; set; }

		long MemberNumber { get; set; }

		long AttackNumber { get; set; }

		bool BlastSpell { get; set; }

		bool UseAttacks { get; set; }

		bool MaxDamage { get; set; }

		bool OmitArmor { get; set; }

		bool OmitSkillGains { get; set; }

		bool OmitMonsterStatus { get; set; }

		bool OmitFinalNewLine { get; set; }

		RTEnums.AttackResult FixedResult { get; set; }

		RTEnums.WeaponRevealType WeaponRevealType { get; set; }

		void ExecuteCalculateDamage(long numDice, long numSides, long mod = 0);

		void ExecuteCheckMonsterStatus();

		void ExecuteAttack();
	}
}
