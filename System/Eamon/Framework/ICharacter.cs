
// ICharacter.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Text;
using Eamon.Framework.Args;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface ICharacter : IGameBase, IComparable<ICharacter>
	{
		#region Properties

		Enums.Gender Gender { get; set; }

		Enums.Status Status { get; set; }

		long[] Stats { get; set; }

		long[] SpellAbilities { get; set; }

		long[] WeaponAbilities { get; set; }

		long ArmorExpertise { get; set; }

		long HeldGold { get; set; }

		long BankGold { get; set; }

		Enums.Armor ArmorClass { get; set; }

		Classes.ICharacterArtifact Armor { get; set; }

		Classes.ICharacterArtifact Shield { get; set; }

		Classes.ICharacterArtifact[] Weapons { get; set; }

		#endregion

		#region Methods

		long GetStats(long index);

		long GetStats(Enums.Stat stat);

		long GetSpellAbilities(long index);

		long GetSpellAbilities(Enums.Spell spell);

		long GetWeaponAbilities(long index);

		long GetWeaponAbilities(Enums.Weapon weapon);

		Classes.ICharacterArtifact GetWeapons(long index);

		string GetSynonyms(long index);

		void SetStats(long index, long value);

		void SetStats(Enums.Stat stat, long value);

		void SetSpellAbilities(long index, long value);

		void SetSpellAbilities(Enums.Spell spell, long value);

		void SetWeaponAbilities(long index, long value);

		void SetWeaponAbilities(Enums.Weapon weapon, long value);

		void SetWeapons(long index, Classes.ICharacterArtifact value);

		void SetSynonyms(long index, string value);

		void ModStats(long index, long value);

		void ModStats(Enums.Stat stat, long value);

		void ModSpellAbilities(long index, long value);

		void ModSpellAbilities(Enums.Spell spell, long value);

		void ModWeaponAbilities(long index, long value);

		void ModWeaponAbilities(Enums.Weapon weapon, long value);

		long GetWeightCarryableGronds();

		long GetWeightCarryableDos();

		long GetIntellectBonusPct();

		long GetCharmMonsterPct();

		long GetMerchantAdjustedCharisma();

		bool IsArmorActive();

		bool IsShieldActive();

		bool IsWeaponActive(long index);

		T EvalGender<T>(T maleValue, T femaleValue, T neutralValue);

		RetCode GetBaseOddsToHit(Classes.ICharacterArtifact weapon, ref long baseOddsToHit);

		RetCode GetBaseOddsToHit(long index, ref long baseOddsToHit);

		RetCode GetWeaponCount(ref long count);

		RetCode ListWeapons(StringBuilder buf, bool capitalize = true);

		void StripPoundCharsFromWeaponNames();

		void AddPoundCharsToWeaponNames();

		RetCode StatDisplay(IStatDisplayArgs args);

		RetCode CopyProperties(ICharacter character);

		#endregion
	}
}
