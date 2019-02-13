
// ICombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework;
using EamonRT.Framework.States;
using RTEnums = EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework.Combat
{
	/// <summary></summary>
	public interface ICombatSystem
	{
		/// <summary></summary>
		Action<IState> SetNextStateFunc { get; set; }

		/// <summary></summary>
		IMonster OfMonster { get; set; }

		/// <summary></summary>
		IMonster DfMonster { get; set; }

		/// <summary></summary>
		long MemberNumber { get; set; }

		/// <summary></summary>
		long AttackNumber { get; set; }

		/// <summary></summary>
		bool BlastSpell { get; set; }

		/// <summary></summary>
		bool UseAttacks { get; set; }

		/// <summary></summary>
		bool MaxDamage { get; set; }

		/// <summary></summary>
		bool OmitArmor { get; set; }

		/// <summary></summary>
		bool OmitSkillGains { get; set; }

		/// <summary></summary>
		bool OmitMonsterStatus { get; set; }

		/// <summary></summary>
		bool OmitFinalNewLine { get; set; }

		/// <summary></summary>
		RTEnums.AttackResult FixedResult { get; set; }

		/// <summary></summary>
		RTEnums.WeaponRevealType WeaponRevealType { get; set; }

		/// <summary></summary>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="mod"></param>
		void ExecuteCalculateDamage(long numDice, long numSides, long mod = 0);

		/// <summary></summary>
		void ExecuteCheckMonsterStatus();

		/// <summary></summary>
		void ExecuteAttack();
	}
}
