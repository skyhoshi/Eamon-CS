﻿
// CharacterHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class CharacterHelper : Helper<ICharacter>, ICharacterHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Stat)i);

			Debug.Assert(stat != null);

			return stat.Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameSpellAbilitiesElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			return string.Format("{0} Spell Ability", spell.Name);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Weapon)i);

			Debug.Assert(weapon != null);

			return string.Format("{0} Wpn Ability", weapon.Name);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameArmorExpertise()
		{
			return "Armor Expertise";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameHeldGold()
		{
			return "Held Gold";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameBankGold()
		{
			return "Bank Gold";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameArmorClass()
		{
			return "Armor Class";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsName()
		{
			var i = Index;

			return string.Format("Wpn #{0} Name", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsIsPlural()
		{
			var i = Index;

			return string.Format("Wpn #{0} Is Plural", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsPluralType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Plural Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsArticleType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Article Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsField1()
		{
			var i = Index;

			return string.Format("Wpn #{0} Complexity", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsField2()
		{
			var i = Index;

			return string.Format("Wpn #{0} Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsField3()
		{
			var i = Index;

			return string.Format("Wpn #{0} Dice", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsField4()
		{
			var i = Index;

			return string.Format("Wpn #{0} Sides", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWeaponsField5()
		{
			var i = Index;

			return string.Format("Wpn #{0} Number Of Hands", i + 1);
		}

		#endregion

		#region GetName Methods

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameStats(bool addToNamesList)
		{
			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				GetName("StatsElement", addToNamesList);
			}

			return "Stats";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameStatsElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Stats[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameSpellAbilities(bool addToNamesList)
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				GetName("SpellAbilitiesElement", addToNamesList);
			}

			return "SpellAbilities";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameSpellAbilitiesElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("SpellAbilities[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponAbilities(bool addToNamesList)
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				GetName("WeaponAbilitiesElement", addToNamesList);
			}

			return "WeaponAbilities";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponAbilitiesElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("WeaponAbilities[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmor(bool addToNamesList)
		{
			GetName("ArmorName", addToNamesList);
			GetName("ArmorDesc", addToNamesList);
			GetName("ArmorIsPlural", addToNamesList);
			GetName("ArmorPluralType", addToNamesList);
			GetName("ArmorArticleType", addToNamesList);
			GetName("ArmorValue", addToNamesList);
			GetName("ArmorWeight", addToNamesList);
			GetName("ArmorType", addToNamesList);
			GetName("ArmorField1", addToNamesList);
			GetName("ArmorField2", addToNamesList);

			return "Armor";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorName(bool addToNamesList)
		{
			var result = "Armor.Name";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorDesc(bool addToNamesList)
		{
			var result = "Armor.Desc";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorIsPlural(bool addToNamesList)
		{
			var result = "Armor.IsPlural";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorPluralType(bool addToNamesList)
		{
			var result = "Armor.PluralType";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorArticleType(bool addToNamesList)
		{
			var result = "Armor.ArticleType";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorValue(bool addToNamesList)
		{
			var result = "Armor.Value";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorWeight(bool addToNamesList)
		{
			var result = "Armor.Weight";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorType(bool addToNamesList)
		{
			var result = "Armor.Type";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorField1(bool addToNamesList)
		{
			var result = "Armor.Field1";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameArmorField2(bool addToNamesList)
		{
			var result = "Armor.Field2";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShield(bool addToNamesList)
		{
			GetName("ShieldName", addToNamesList);
			GetName("ShieldDesc", addToNamesList);
			GetName("ShieldIsPlural", addToNamesList);
			GetName("ShieldPluralType", addToNamesList);
			GetName("ShieldArticleType", addToNamesList);
			GetName("ShieldValue", addToNamesList);
			GetName("ShieldWeight", addToNamesList);
			GetName("ShieldType", addToNamesList);
			GetName("ShieldField1", addToNamesList);
			GetName("ShieldField2", addToNamesList);

			return "Shield";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldName(bool addToNamesList)
		{
			var result = "Shield.Name";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldDesc(bool addToNamesList)
		{
			var result = "Shield.Desc";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldIsPlural(bool addToNamesList)
		{
			var result = "Shield.IsPlural";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldPluralType(bool addToNamesList)
		{
			var result = "Shield.PluralType";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldArticleType(bool addToNamesList)
		{
			var result = "Shield.ArticleType";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldValue(bool addToNamesList)
		{
			var result = "Shield.Value";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldWeight(bool addToNamesList)
		{
			var result = "Shield.Weight";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldType(bool addToNamesList)
		{
			var result = "Shield.Type";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldField1(bool addToNamesList)
		{
			var result = "Shield.Field1";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameShieldField2(bool addToNamesList)
		{
			var result = "Shield.Field2";

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeapons(bool addToNamesList)
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				GetName("WeaponsName", addToNamesList);
				GetName("WeaponsDesc", addToNamesList);
				GetName("WeaponsIsPlural", addToNamesList);
				GetName("WeaponsPluralType", addToNamesList);
				GetName("WeaponsArticleType", addToNamesList);
				GetName("WeaponsValue", addToNamesList);
				GetName("WeaponsWeight", addToNamesList);
				GetName("WeaponsType", addToNamesList);
				GetName("WeaponsField1", addToNamesList);
				GetName("WeaponsField2", addToNamesList);
				GetName("WeaponsField3", addToNamesList);
				GetName("WeaponsField4", addToNamesList);
				GetName("WeaponsField5", addToNamesList);
			}

			return "Weapons";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsName(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Name", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsDesc(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Desc", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsIsPlural(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].IsPlural", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsPluralType(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].PluralType", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsArticleType(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].ArticleType", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsValue(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Value", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsWeight(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Weight", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsType(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Type", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsField1(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field1", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsField2(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field2", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsField3(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field3", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsField4(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field4", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameWeaponsField5(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field5", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueStatsElement()
		{
			var i = Index;

			return Record.GetStats(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueSpellAbilitiesElement()
		{
			var i = Index;

			return Record.GetSpellAbilities(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponAbilitiesElement()
		{
			var i = Index;

			return Record.GetWeaponAbilities(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorName()
		{
			return Record.Armor.Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorDesc()
		{
			return Record.Armor.Desc;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorIsPlural()
		{
			return Record.Armor.IsPlural;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorPluralType()
		{
			return Record.Armor.PluralType;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorArticleType()
		{
			return Record.Armor.ArticleType;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorValue()
		{
			return Record.Armor.Value;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorWeight()
		{
			return Record.Armor.Weight;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorType()
		{
			return Record.Armor.Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorField1()
		{
			return Record.Armor.Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueArmorField2()
		{
			return Record.Armor.Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldName()
		{
			return Record.Shield.Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldDesc()
		{
			return Record.Shield.Desc;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldIsPlural()
		{
			return Record.Shield.IsPlural;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldPluralType()
		{
			return Record.Shield.PluralType;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldArticleType()
		{
			return Record.Shield.ArticleType;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldValue()
		{
			return Record.Shield.Value;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldWeight()
		{
			return Record.Shield.Weight;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldType()
		{
			return Record.Shield.Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldField1()
		{
			return Record.Shield.Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueShieldField2()
		{
			return Record.Shield.Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsName()
		{
			var i = Index;

			return Record.GetWeapons(i).Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsDesc()
		{
			var i = Index;

			return Record.GetWeapons(i).Desc;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsIsPlural()
		{
			var i = Index;

			return Record.GetWeapons(i).IsPlural;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsPluralType()
		{
			var i = Index;

			return Record.GetWeapons(i).PluralType;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsArticleType()
		{
			var i = Index;

			return Record.GetWeapons(i).ArticleType;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsValue()
		{
			var i = Index;

			return Record.GetWeapons(i).Value;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsWeight()
		{
			var i = Index;

			return Record.GetWeapons(i).Weight;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsType()
		{
			var i = Index;

			return Record.GetWeapons(i).Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsField1()
		{
			var i = Index;

			return Record.GetWeapons(i).Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsField2()
		{
			var i = Index;

			return Record.GetWeapons(i).Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsField3()
		{
			var i = Index;

			return Record.GetWeapons(i).Field3;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsField4()
		{
			var i = Index;

			return Record.GetWeapons(i).Field4;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueWeaponsField5()
		{
			var i = Index;

			return Record.GetWeapons(i).Field5;
		}

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateName()
		{
			if (Record.Name != null)
			{
				Record.Name = Regex.Replace(Record.Name, @"\s+", " ").Trim();
			}

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.CharNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArticleType()
		{
			return Record.ArticleType == ArticleType.None;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateGender()
		{
			return Enum.IsDefined(typeof(Gender), Record.Gender);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateStatus()
		{
			return Enum.IsDefined(typeof(Status), Record.Status);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateStats()
		{
			var result = true;

			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				result = ValidateField("StatsElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Stat)i);

			Debug.Assert(stat != null);

			return Record.GetStats(i) >= stat.MinValue && Record.GetStats(i) <= stat.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSpellAbilities()
		{
			var result = true;

			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				result = ValidateField("SpellAbilitiesElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSpellAbilitiesElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSpellAbilities(i) >= spell.MinValue && Record.GetSpellAbilities(i) <= spell.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponAbilities()
		{
			var result = true;

			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				result = ValidateField("WeaponAbilitiesElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Weapon)i);

			Debug.Assert(weapon != null);

			return Record.GetWeaponAbilities(i) >= weapon.MinValue && Record.GetWeaponAbilities(i) <= weapon.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorExpertise()
		{
			return Record.ArmorExpertise >= 0 && Record.ArmorExpertise <= 79;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateHeldGold()
		{
			return Record.HeldGold >= Constants.MinGoldValue && Record.HeldGold <= Constants.MaxGoldValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateBankGold()
		{
			return Record.BankGold >= Constants.MinGoldValue && Record.BankGold <= Constants.MaxGoldValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorClass()
		{
			return Enum.IsDefined(typeof(Armor), Record.ArmorClass);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmor()
		{
			var result = true;

			result = ValidateField("ArmorName") &&
							ValidateField("ArmorDesc") &&
							ValidateField("ArmorIsPlural") &&
							ValidateField("ArmorPluralType") &&
							ValidateField("ArmorArticleType") &&
							ValidateField("ArmorValue") &&
							ValidateField("ArmorWeight") &&
							ValidateField("ArmorType") &&
							ValidateField("ArmorField1") &&
							ValidateField("ArmorField2");

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorName()
		{
			var result = true;

			if (Record.Armor.Name != null)
			{
				Record.Armor.Name = Regex.Replace(Record.Armor.Name, @"\s+", " ").Trim();
			}

			if (Record.IsArmorActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Armor.Name) && Record.Armor.Name.Length <= Constants.CharArtNameLen;
			}
			else
			{
				result = Record.Armor.Name != null && (Record.Armor.Name == "" || string.Equals(Record.Armor.Name, "NONE", StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorDesc()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Armor.Desc) && Record.Armor.Desc.Length <= Constants.CharArtDescLen;
			}
			else
			{
				result = Record.Armor.Desc == "";
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorIsPlural()
		{
			var result = true;

			if (!Record.IsArmorActive())
			{
				result = Record.Armor.IsPlural == false;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorPluralType()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Enum.IsDefined(typeof(PluralType), Record.Armor.PluralType);
			}
			else
			{
				result = Record.Armor.PluralType == PluralType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorArticleType()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Enum.IsDefined(typeof(ArticleType), Record.Armor.ArticleType);
			}
			else
			{
				result = Record.Armor.ArticleType == ArticleType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorValue()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Record.Armor.Value >= Constants.MinGoldValue && Record.Armor.Value <= Constants.MaxGoldValue;
			}
			else
			{
				result = Record.Armor.Value == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorWeight()
		{
			var result = true;

			if (!Record.IsArmorActive())
			{
				result = Record.Armor.Weight == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorType()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Record.Armor.Type == ArtifactType.Wearable;
			}
			else
			{
				result = Record.Armor.Type == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorField1()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Globals.Engine.IsValidArtifactArmor(Record.Armor.Field1);
			}
			else
			{
				result = Record.Armor.Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArmorField2()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Enum.IsDefined(typeof(Clothing), Record.Armor.Field2);
			}
			else
			{
				result = Record.Armor.Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShield()
		{
			var result = true;

			result = ValidateField("ShieldName") &&
							ValidateField("ShieldDesc") &&
							ValidateField("ShieldIsPlural") &&
							ValidateField("ShieldPluralType") &&
							ValidateField("ShieldArticleType") &&
							ValidateField("ShieldValue") &&
							ValidateField("ShieldWeight") &&
							ValidateField("ShieldType") &&
							ValidateField("ShieldField1") &&
							ValidateField("ShieldField2");

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldName()
		{
			var result = true;

			if (Record.Shield.Name != null)
			{
				Record.Shield.Name = Regex.Replace(Record.Shield.Name, @"\s+", " ").Trim();
			}

			if (Record.IsShieldActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Shield.Name) && Record.Shield.Name.Length <= Constants.CharArtNameLen;
			}
			else
			{
				result = Record.Shield.Name != null && (Record.Shield.Name == "" || string.Equals(Record.Shield.Name, "NONE", StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldDesc()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Shield.Desc) && Record.Shield.Desc.Length <= Constants.CharArtDescLen;
			}
			else
			{
				result = Record.Shield.Desc == "";
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldIsPlural()
		{
			var result = true;

			if (!Record.IsShieldActive())
			{
				result = Record.Shield.IsPlural == false;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldPluralType()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Enum.IsDefined(typeof(PluralType), Record.Shield.PluralType);
			}
			else
			{
				result = Record.Shield.PluralType == PluralType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldArticleType()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Enum.IsDefined(typeof(ArticleType), Record.Shield.ArticleType);
			}
			else
			{
				result = Record.Shield.ArticleType == ArticleType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldValue()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Record.Shield.Value >= Constants.MinGoldValue && Record.Shield.Value <= Constants.MaxGoldValue;
			}
			else
			{
				result = Record.Shield.Value == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldWeight()
		{
			var result = true;

			if (!Record.IsShieldActive())
			{
				result = Record.Shield.Weight == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldType()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Record.Shield.Type == ArtifactType.Wearable;
			}
			else
			{
				result = Record.Shield.Type == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldField1()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Globals.Engine.IsValidArtifactArmor(Record.Shield.Field1);
			}
			else
			{
				result = Record.Shield.Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateShieldField2()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Enum.IsDefined(typeof(Clothing), Record.Shield.Field2);
			}
			else
			{
				result = Record.Shield.Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeapons()
		{
			var result = true;

			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				result = ValidateField("WeaponsName") &&
								ValidateField("WeaponsDesc") &&
								ValidateField("WeaponsIsPlural") &&
								ValidateField("WeaponsPluralType") &&
								ValidateField("WeaponsArticleType") &&
								ValidateField("WeaponsValue") &&
								ValidateField("WeaponsWeight") &&
								ValidateField("WeaponsType") &&
								ValidateField("WeaponsField1") &&
								ValidateField("WeaponsField2") &&
								ValidateField("WeaponsField3") &&
								ValidateField("WeaponsField4") &&
								ValidateField("WeaponsField5");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsName()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			if (Record.GetWeapons(i).Name != null)
			{
				Record.GetWeapons(i).Name = Regex.Replace(Record.GetWeapons(i).Name, @"\s+", " ").Trim();
			}

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				if (Record.GetWeapons(i).Name.Length > Constants.CharArtNameLen)
				{
					for (var j = Constants.CharArtNameLen; j < Record.GetWeapons(i).Name.Length; j++)
					{
						if (Record.GetWeapons(i).Name[j] != '#')
						{
							result = false;

							break;
						}
					}
				}
			}
			else
			{
				result = Record.GetWeapons(i).Name != null && (Record.GetWeapons(i).Name == "" || string.Equals(Record.GetWeapons(i).Name, "NONE", StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsDesc()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapons(i).Desc == "" || (!string.IsNullOrWhiteSpace(Record.GetWeapons(i).Desc) && Record.GetWeapons(i).Desc.Length <= Constants.CharArtDescLen);
			}
			else
			{
				result = Record.GetWeapons(i).Desc == "";
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsIsPlural()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (!activeWeapon)
			{
				result = Record.GetWeapons(i).IsPlural == false;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsPluralType()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(PluralType), Record.GetWeapons(i).PluralType);
			}
			else
			{
				result = Record.GetWeapons(i).PluralType == PluralType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsArticleType()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(ArticleType), Record.GetWeapons(i).ArticleType);
			}
			else
			{
				result = Record.GetWeapons(i).ArticleType == ArticleType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsValue()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapons(i).Value >= Constants.MinGoldValue && Record.GetWeapons(i).Value <= Constants.MaxGoldValue;
			}
			else
			{
				result = Record.GetWeapons(i).Value == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsWeight()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (!activeWeapon)
			{
				result = Record.GetWeapons(i).Weight == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsType()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapons(i).Type == 0 || Record.GetWeapons(i).Type == ArtifactType.Weapon || Record.GetWeapons(i).Type == ArtifactType.MagicWeapon;
			}
			else
			{
				result = Record.GetWeapons(i).Type == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsField1()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapons(i).Field1 >= -50 && Record.GetWeapons(i).Field1 <= 50;
			}
			else
			{
				result = Record.GetWeapons(i).Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsField2()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(Weapon), Record.GetWeapons(i).Field2);
			}
			else
			{
				result = Record.GetWeapons(i).Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsField3()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapons(i).Field3 >= 1 && Record.GetWeapons(i).Field3 <= 25;
			}
			else
			{
				result = Record.GetWeapons(i).Field3 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsField4()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapons(i).Field4 >= 1 && Record.GetWeapons(i).Field4 <= 25;
			}
			else
			{
				result = Record.GetWeapons(i).Field4 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeaponsField5()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				if (Record.GetWeapons(i).Field5 == 0)  // auto-upgrade old weapons
				{
					Record.GetWeapons(i).Field5 = Record.GetWeapons(i).Field2 == (long)Weapon.Bow ? 2 : 1;
				}

				result = Record.GetWeapons(i).Field5 >= 1 && Record.GetWeapons(i).Field5 <= 2;
			}
			else
			{
				result = Record.GetWeapons(i).Field5 == 0;
			}

			return result;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		protected virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the character.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescGender()
		{
			var fullDesc = "Enter the gender of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var genderValues = EnumUtil.GetValues<Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], Globals.Engine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		protected virtual void PrintDescStatus()
		{
			var fullDesc = "Enter the status of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var statusValues = EnumUtil.GetValues<Status>();

			for (var j = 0; j < statusValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)statusValues[j], Globals.Engine.GetStatusNames(statusValues[j]));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		protected virtual void PrintDescStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Stat)i);

			Debug.Assert(stat != null);

			var fullDesc = string.Format("Enter the {0} of the character.", stat.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", stat.MinValue, stat.MaxValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescSpellAbilitiesElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			var fullDesc = string.Format("Enter the character's {0} spell ability.", spell.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", spell.MinValue, spell.MaxValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Weapon)i);

			Debug.Assert(weapon != null);

			var fullDesc = string.Format("Enter the character's {0} weapon ability.", weapon.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", weapon.MinValue, weapon.MaxValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescArmorExpertise()
		{
			var fullDesc = "Enter the armor expertise of the character.";

			var briefDesc = "0-79=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescHeldGold()
		{
			var fullDesc = "Enter the character's gold in hand.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescBankGold()
		{
			var fullDesc = "Enter the character's gold in the bank.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescArmorClass()
		{
			var fullDesc = "Enter the armor class of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var armorValues = EnumUtil.GetValues<Armor>();

			for (var j = 0; j < armorValues.Count; j++)
			{
				var armor = Globals.Engine.GetArmors(armorValues[j]);

				Debug.Assert(armor != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsName()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} name.", i + 1);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsIsPlural()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} Is Plural status.", i + 1);

			var briefDesc = "0=Singular; 1=Plural";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsPluralType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} plural type.", i + 1);

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsArticleType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} article type.", i + 1);

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsField1()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} complexity.", i + 1);

			var briefDesc = "-50-50=Valid value";          // TODO: eliminate hardcode

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsField2()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} type.", i + 1);

			var briefDesc = new StringBuilder(Constants.BufSize);

			var weaponValues = EnumUtil.GetValues<Weapon>();

			for (var j = 0; j < weaponValues.Count; j++)
			{
				var weapon = Globals.Engine.GetWeapons(weaponValues[j]);

				Debug.Assert(weapon != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsField3()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} hit dice.", i + 1);

			var briefDesc = "1-25=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsField4()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} hit dice sides.", i + 1);

			var briefDesc = "1-25=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescWeaponsField5()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} number of hands required.", i + 1);

			var briefDesc = "1-2=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		#endregion

		#region List Methods

		/// <summary></summary>
		protected virtual void ListUid()
		{
			if (FullDetail)
			{
				if (!ExcludeROFields)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Globals.Engine.Capitalize(Record.Name));
			}
		}

		/// <summary></summary>
		protected virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		/// <summary></summary>
		protected virtual void ListGender()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null), Record.EvalGender("Male", "Female", "Neutral"));
			}
		}

		/// <summary></summary>
		protected virtual void ListStatus()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Status"), null), Globals.Engine.GetStatusNames(Record.Status));
			}
		}

		/// <summary></summary>
		protected virtual void ListStats()
		{
			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				ListField("StatsElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		protected virtual void ListStatsElement()
		{
			var i = Index;

			var sv = (Stat)i;

			if (FullDetail)
			{
				if (LookupMsg)
				{
					Buf.Clear();

					if (sv == Stat.Intellect)
					{
						var ibp = Record.GetIntellectBonusPct();

						Buf.AppendFormat("Learning: {0}{1}%", ibp > 0 ? "+" : "", ibp);
					}
					else if (sv == Stat.Hardiness)
					{
						Buf.AppendFormat("Weight Carryable: {0} G ({1} D)", Record.GetWeightCarryableGronds(), Record.GetWeightCarryableDos());
					}
					else if (sv == Stat.Charisma)
					{
						var cmp = Record.GetCharmMonsterPct();

						Buf.AppendFormat("Charm Monster: {0}{1}%", cmp > 0 ? "+" : "", cmp);
					}
				}

				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Buf.Length > 0)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("StatsElement"), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.GetStats(i), null, Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("StatsElement"), null),
						Record.GetStats(i));
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListSpellAbilities()
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				ListField("SpellAbilitiesElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		protected virtual void ListSpellAbilitiesElement()
		{
			var i = Index;

			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}%",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("SpellAbilitiesElement"), null),
					Record.GetSpellAbilities(i));
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponAbilities()
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				ListField("WeaponAbilitiesElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		protected virtual void ListWeaponAbilitiesElement()
		{
			var i = Index;

			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}%",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponAbilitiesElement"), null),
					Record.GetWeaponAbilities(i));
			}
		}

		/// <summary></summary>
		protected virtual void ListArmorExpertise()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArmorExpertise"), null), Record.ArmorExpertise);
			}
		}

		/// <summary></summary>
		protected virtual void ListHeldGold()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("HeldGold"), null), Record.HeldGold);
			}
		}

		/// <summary></summary>
		protected virtual void ListBankGold()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("BankGold"), null), Record.BankGold);
			}
		}

		/// <summary></summary>
		protected virtual void ListArmorClass()
		{
			if (FullDetail)
			{
				var armor = Globals.Engine.GetArmors(Record.ArmorClass);

				Debug.Assert(armor != null);

				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArmorClass"), null), armor.Name);
			}
		}

		/// <summary></summary>
		protected virtual void ListWeapons()
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				ListField("WeaponsName");
				ListField("WeaponsIsPlural");
				ListField("WeaponsPluralType");
				ListField("WeaponsArticleType");
				ListField("WeaponsField1");
				ListField("WeaponsField2");
				ListField("WeaponsField3");
				ListField("WeaponsField4");
				ListField("WeaponsField5");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		protected virtual void ListWeaponsName()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || i == 0 || Record.IsWeaponActive(i - 1))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsName"), null), Record.GetWeapons(i).Name);
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsIsPlural()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsIsPlural"), null), Convert.ToInt64(Record.GetWeapons(i).IsPlural));
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsPluralType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsPluralType"), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.GetWeapons(i).PluralType, null,
							Record.GetWeapons(i).PluralType == PluralType.None ? "No change" :
							Record.GetWeapons(i).PluralType == PluralType.S ? "Use 's'" :
							Record.GetWeapons(i).PluralType == PluralType.Es ? "Use 'es'" :
							Record.GetWeapons(i).PluralType == PluralType.YIes ? "Use 'y' to 'ies'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsPluralType"), null), (long)Record.GetWeapons(i).PluralType);
					}
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsArticleType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsArticleType"), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.GetWeapons(i).ArticleType, null,
							Record.GetWeapons(i).ArticleType == ArticleType.None ? "No article" :
							Record.GetWeapons(i).ArticleType == ArticleType.A ? "Use 'a'" :
							Record.GetWeapons(i).ArticleType == ArticleType.An ? "Use 'an'" :
							Record.GetWeapons(i).ArticleType == ArticleType.Some ? "Use 'some'" :
							Record.GetWeapons(i).ArticleType == ArticleType.The ? "Use 'the'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsArticleType"), null), (long)Record.GetWeapons(i).ArticleType);
					}
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsField1()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField1"), null), Record.GetWeapons(i).Field1);
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsField2()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var weapon = Globals.Engine.GetWeapons((Weapon)Record.GetWeapons(i).Field2);

					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField2"), null),
						weapon != null ? weapon.Name : "0");
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsField3()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField3"), null), Record.GetWeapons(i).Field3);
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsField4()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField4"), null), Record.GetWeapons(i).Field4);
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListWeaponsField5()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField5"), null), Record.GetWeapons(i).Field5);
				}
			}
		}

		#endregion

		#region Input Methods

		/// <summary></summary>
		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputName()
		{
			var fieldDesc = FieldDesc;

			var name = Record.Name;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", name);

				PrintFieldDesc("Name", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Name"), null));

				var rc = Globals.In.ReadField(Buf, Constants.CharNameLen, null, '_', '\0', false, null, null, Globals.Engine.IsCharAnyButDquoteCommaColon, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = Buf.ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputGender()
		{
			var fieldDesc = FieldDesc;

			var gender = Record.Gender;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc("Gender", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Gender"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Gender = (Gender)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Gender"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputStatus()
		{
			var fieldDesc = FieldDesc;

			var status = Record.Status;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)status);

				PrintFieldDesc("Status", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Status"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0To3, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Status = (Status)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Status"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputStats()
		{
			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				InputField("StatsElement");
			}
		}

		/// <summary></summary>
		protected virtual void InputStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Stat)i);

			Debug.Assert(stat != null);

			var fieldDesc = FieldDesc;

			var value = Record.GetStats(i);

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("StatsElement", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("StatsElement"), stat.EmptyVal));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, stat.EmptyVal, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.SetStats(i, Convert.ToInt64(Buf.Trim().ToString()));

				if (ValidateField("StatsElement"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputSpellAbilities()
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				InputField("SpellAbilitiesElement");
			}
		}

		/// <summary></summary>
		protected virtual void InputSpellAbilitiesElement()
		{
			var i = Index;

			var fieldDesc = FieldDesc;

			var value = Record.GetSpellAbilities(i);

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("SpellAbilitiesElement", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("SpellAbilitiesElement"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.SetSpellAbilities(i, Convert.ToInt64(Buf.Trim().ToString()));

				if (ValidateField("SpellAbilitiesElement"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputWeaponAbilities()
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				InputField("WeaponAbilitiesElement");
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Weapon)i);

			Debug.Assert(weapon != null);

			var fieldDesc = FieldDesc;

			var value = Record.GetWeaponAbilities(i);

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("WeaponAbilitiesElement", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponAbilitiesElement"), weapon.EmptyVal));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, weapon.EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.SetWeaponAbilities(i, Convert.ToInt64(Buf.Trim().ToString()));
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("WeaponAbilitiesElement"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputArmorExpertise()
		{
			var fieldDesc = FieldDesc;

			var armorExpertise = Record.ArmorExpertise;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", armorExpertise);

				PrintFieldDesc("ArmorExpertise", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ArmorExpertise"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArmorExpertise = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArmorExpertise"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputHeldGold()
		{
			var fieldDesc = FieldDesc;

			var heldGold = Record.HeldGold;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", heldGold);

				PrintFieldDesc("HeldGold", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("HeldGold"), "200"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "200", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.HeldGold = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("HeldGold"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputBankGold()
		{
			var fieldDesc = FieldDesc;

			var bankGold = Record.BankGold;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", bankGold);

				PrintFieldDesc("BankGold", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("BankGold"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.BankGold = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("BankGold"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputArmorClass()
		{
			var fieldDesc = FieldDesc;

			var armorClass = Record.ArmorClass;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)armorClass);

				PrintFieldDesc("ArmorClass", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ArmorClass"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArmorClass = (Armor)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArmorClass"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			if (Record.ArmorClass != armorClass)
			{
				Record.Armor = Globals.CreateInstance<ICharacterArtifact>(x =>
				{
					x.Parent = Record;
				});

				Record.Shield = Globals.CreateInstance<ICharacterArtifact>(x =>
				{
					x.Parent = Record;
				});
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputWeapons()
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				InputField("WeaponsName");
				InputField("WeaponsIsPlural");
				InputField("WeaponsPluralType");
				InputField("WeaponsArticleType");
				InputField("WeaponsField1");
				InputField("WeaponsField2");
				InputField("WeaponsField3");
				InputField("WeaponsField4");
				InputField("WeaponsField5");
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsName()
		{
			var i = Index;

			if (i == 0 || Record.IsWeaponActive(i - 1))
			{
				var fieldDesc = FieldDesc;

				var name = Record.GetWeapons(i).Name;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", name);

					PrintFieldDesc("WeaponsName", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsName"), null));

					var rc = Globals.In.ReadField(Buf, Constants.CharArtNameLen, null, '_', '\0', true, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Name = Buf.ToString();

					if (ValidateField("WeaponsName"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.IsWeaponActive(i))
				{
					var clearExtraFields = !string.Equals(Record.GetWeapons(i).Name, name, StringComparison.OrdinalIgnoreCase);

					if (EditRec && (Record.GetWeapons(i).Field3 == 0 || Record.GetWeapons(i).Field4 == 0))
					{
						Record.GetWeapons(i).IsPlural = false;

						Record.GetWeapons(i).PluralType = PluralType.S;

						Record.GetWeapons(i).ArticleType = ArticleType.A;

						Record.GetWeapons(i).Field1 = 5;

						Record.GetWeapons(i).Field2 = (long)Weapon.Sword;

						Record.GetWeapons(i).Field3 = 1;

						Record.GetWeapons(i).Field4 = 6;

						Record.GetWeapons(i).Field5 = 1;

						clearExtraFields = true;
					}

					if (clearExtraFields)
					{
						Record.GetWeapons(i).ClearExtraFields();
					}
				}
				else
				{
					for (var k = i; k < Record.Weapons.Length; k++)
					{
						Record.GetWeapons(k).Name = "NONE";

						Record.GetWeapons(k).IsPlural = false;

						Record.GetWeapons(k).PluralType = 0;

						Record.GetWeapons(k).ArticleType = 0;

						Record.GetWeapons(k).Field1 = 0;

						Record.GetWeapons(k).Field2 = 0;

						Record.GetWeapons(k).Field3 = 0;

						Record.GetWeapons(k).Field4 = 0;

						Record.GetWeapons(k).Field5 = 0;

						Record.GetWeapons(k).ClearExtraFields();
					}
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Name = "NONE";

				Record.GetWeapons(i).ClearExtraFields();
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsIsPlural()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var isPlural = Record.GetWeapons(i).IsPlural;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

					PrintFieldDesc("WeaponsIsPlural", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsIsPlural"), "0"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).IsPlural = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

					if (ValidateField("WeaponsIsPlural"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).IsPlural != isPlural)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).IsPlural = false;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsPluralType()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var pluralType = Record.GetWeapons(i).PluralType;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)pluralType);

					PrintFieldDesc("WeaponsPluralType", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsPluralType"), "1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).PluralType = (PluralType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsPluralType"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).PluralType != pluralType)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).PluralType = 0;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsArticleType()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var articleType = Record.GetWeapons(i).ArticleType;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)articleType);

					PrintFieldDesc("WeaponsArticleType", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsArticleType"), "1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).ArticleType = (ArticleType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsArticleType"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).ArticleType != articleType)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).ArticleType = 0;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsField1()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field1 = Record.GetWeapons(i).Field1;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field1);

					PrintFieldDesc("WeaponsField1", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField1"), "5"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).Field1 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsField1"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).Field1 != field1)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Field1 = 0;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsField2()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field2 = Record.GetWeapons(i).Field2;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field2);

					PrintFieldDesc("WeaponsField2", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField2"), "5"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Field2 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField2"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).Field2 != field2)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Field2 = 0;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsField3()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field3 = Record.GetWeapons(i).Field3;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field3);

					PrintFieldDesc("WeaponsField3", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField3"), "1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Field3 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField3"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).Field3 != field3)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Field3 = 0;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsField4()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field4 = Record.GetWeapons(i).Field4;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field4);

					PrintFieldDesc("WeaponsField4", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField4"), "6"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Field4 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField4"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).Field4 != field4)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Field4 = 0;
			}
		}

		/// <summary></summary>
		protected virtual void InputWeaponsField5()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field5 = Record.GetWeapons(i).Field5;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field5);

					PrintFieldDesc("WeaponsField5", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField5"), "1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Field5 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField5"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapons(i).Field5 != field5)
				{
					Record.GetWeapons(i).ClearExtraFields();
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Field5 = 0;
			}
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class CharacterHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetCharacterUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#endregion

		#region Class CharacterHelper

		public CharacterHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"Seen",
				"ArticleType",
				"Gender",
				"Status",
				"Stats",
				"SpellAbilities",
				"WeaponAbilities",
				"ArmorExpertise",
				"HeldGold",
				"BankGold",
				"ArmorClass",
				"Armor",
				"Shield",
				"Weapons",
			};
		}

		#endregion

		#endregion
	}
}
