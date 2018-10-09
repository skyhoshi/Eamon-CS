
// CharacterHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class CharacterHelper : Helper<ICharacter>, ICharacterHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		protected virtual string GetPrintedNameStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			return stat.Name;
		}

		protected virtual string GetPrintedNameSpellAbilitiesElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return string.Format("{0} Spell Ability", spell.Name);
		}

		protected virtual string GetPrintedNameWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			return string.Format("{0} Wpn Ability", weapon.Name);
		}

		protected virtual string GetPrintedNameArmorExpertise()
		{
			return "Armor Expertise";
		}

		protected virtual string GetPrintedNameHeldGold()
		{
			return "Held Gold";
		}

		protected virtual string GetPrintedNameBankGold()
		{
			return "Bank Gold";
		}

		protected virtual string GetPrintedNameArmorClass()
		{
			return "Armor Class";
		}

		protected virtual string GetPrintedNameWeaponsName()
		{
			var i = Index;

			return string.Format("Wpn #{0} Name", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsIsPlural()
		{
			var i = Index;

			return string.Format("Wpn #{0} Is Plural", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsPluralType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Plural Type", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsArticleType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Article Type", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsComplexity()
		{
			var i = Index;

			return string.Format("Wpn #{0} Complexity", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Type", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsDice()
		{
			var i = Index;

			return string.Format("Wpn #{0} Dice", i + 1);
		}

		protected virtual string GetPrintedNameWeaponsSides()
		{
			var i = Index;

			return string.Format("Wpn #{0} Sides", i + 1);
		}

		#endregion

		#region GetName Methods

		protected virtual string GetNameStats(bool addToNamesList)
		{
			var statValues = EnumUtil.GetValues<Enums.Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				GetName("StatsElement", addToNamesList);
			}

			return "Stats";
		}

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

		protected virtual string GetNameSpellAbilities(bool addToNamesList)
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				GetName("SpellAbilitiesElement", addToNamesList);
			}

			return "SpellAbilities";
		}

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

		protected virtual string GetNameWeaponAbilities(bool addToNamesList)
		{
			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				GetName("WeaponAbilitiesElement", addToNamesList);
			}

			return "WeaponAbilities";
		}

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

		protected virtual string GetNameWeapons(bool addToNamesList)
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				GetName("WeaponsName", addToNamesList);
				GetName("WeaponsIsPlural", addToNamesList);
				GetName("WeaponsPluralType", addToNamesList);
				GetName("WeaponsArticleType", addToNamesList);
				GetName("WeaponsComplexity", addToNamesList);
				GetName("WeaponsType", addToNamesList);
				GetName("WeaponsDice", addToNamesList);
				GetName("WeaponsSides", addToNamesList);
			}

			return "Weapons";
		}

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

		protected virtual string GetNameWeaponsComplexity(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Complexity", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

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

		protected virtual string GetNameWeaponsDice(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Dice", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameWeaponsSides(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Sides", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		protected virtual object GetValueStatsElement()
		{
			var i = Index;

			return Record.GetStats(i);
		}

		protected virtual object GetValueSpellAbilitiesElement()
		{
			var i = Index;

			return Record.GetSpellAbilities(i);
		}

		protected virtual object GetValueWeaponAbilitiesElement()
		{
			var i = Index;

			return Record.GetWeaponAbilities(i);
		}

		protected virtual object GetValueWeaponsName()
		{
			var i = Index;

			return Record.GetWeapons(i).Name;
		}

		protected virtual object GetValueWeaponsIsPlural()
		{
			var i = Index;

			return Record.GetWeapons(i).IsPlural;
		}

		protected virtual object GetValueWeaponsPluralType()
		{
			var i = Index;

			return Record.GetWeapons(i).PluralType;
		}

		protected virtual object GetValueWeaponsArticleType()
		{
			var i = Index;

			return Record.GetWeapons(i).ArticleType;
		}

		protected virtual object GetValueWeaponsComplexity()
		{
			var i = Index;

			return Record.GetWeapons(i).Complexity;
		}

		protected virtual object GetValueWeaponsType()
		{
			var i = Index;

			return Record.GetWeapons(i).Type;
		}

		protected virtual object GetValueWeaponsDice()
		{
			var i = Index;

			return Record.GetWeapons(i).Dice;
		}

		protected virtual object GetValueWeaponsSides()
		{
			var i = Index;

			return Record.GetWeapons(i).Sides;
		}

		#endregion

		#region Validate Methods

		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateName()
		{
			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.CharNameLen;
		}

		protected virtual bool ValidateArticleType()
		{
			return Record.ArticleType == Enums.ArticleType.None;
		}

		protected virtual bool ValidateGender()
		{
			return Enum.IsDefined(typeof(Enums.Gender), Record.Gender);
		}

		protected virtual bool ValidateStatus()
		{
			return Enum.IsDefined(typeof(Enums.Status), Record.Status);
		}

		protected virtual bool ValidateStats()
		{
			var result = true;

			var statValues = EnumUtil.GetValues<Enums.Stat>();

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

		protected virtual bool ValidateStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			return Record.GetStats(i) >= stat.MinValue && Record.GetStats(i) <= stat.MaxValue;
		}

		protected virtual bool ValidateSpellAbilities()
		{
			var result = true;

			var spellValues = EnumUtil.GetValues<Enums.Spell>();

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

		protected virtual bool ValidateSpellAbilitiesElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSpellAbilities(i) >= spell.MinValue && Record.GetSpellAbilities(i) <= spell.MaxValue;
		}

		protected virtual bool ValidateWeaponAbilities()
		{
			var result = true;

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

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

		protected virtual bool ValidateWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			return Record.GetWeaponAbilities(i) >= weapon.MinValue && Record.GetWeaponAbilities(i) <= weapon.MaxValue;
		}

		protected virtual bool ValidateArmorExpertise()
		{
			return Record.ArmorExpertise >= 0 && Record.ArmorExpertise <= 79;
		}

		protected virtual bool ValidateHeldGold()
		{
			return Record.HeldGold >= Constants.MinGoldValue && Record.HeldGold <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateBankGold()
		{
			return Record.BankGold >= Constants.MinGoldValue && Record.BankGold <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateArmorClass()
		{
			return Enum.IsDefined(typeof(Enums.Armor), Record.ArmorClass);
		}

		protected virtual bool ValidateWeapons()
		{
			var result = true;

			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				result = ValidateField("WeaponsName") &&
								ValidateField("WeaponsIsPlural") &&
								ValidateField("WeaponsPluralType") &&
								ValidateField("WeaponsArticleType") &&
								ValidateField("WeaponsComplexity") &&
								ValidateField("WeaponsType") &&
								ValidateField("WeaponsDice") &&
								ValidateField("WeaponsSides");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateWeaponsName()
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
				if (Record.GetWeapons(i).Name.Length > Constants.CharWpnNameLen)
				{
					for (var j = Constants.CharWpnNameLen; j < Record.GetWeapons(i).Name.Length; j++)
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
				result = Enum.IsDefined(typeof(Enums.PluralType), Record.GetWeapons(i).PluralType);
			}
			else
			{
				result = Record.GetWeapons(i).PluralType == Enums.PluralType.None;
			}

			return result;
		}

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
				result = Enum.IsDefined(typeof(Enums.ArticleType), Record.GetWeapons(i).ArticleType);
			}
			else
			{
				result = Record.GetWeapons(i).ArticleType == Enums.ArticleType.None;
			}

			return result;
		}

		protected virtual bool ValidateWeaponsComplexity()
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
				result = Record.GetWeapons(i).Complexity >= -50 && Record.GetWeapons(i).Complexity <= 50;
			}
			else
			{
				result = Record.GetWeapons(i).Complexity == 0;
			}

			return result;
		}

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
				result = Enum.IsDefined(typeof(Enums.Weapon), Record.GetWeapons(i).Type);
			}
			else
			{
				result = Record.GetWeapons(i).Type == 0;
			}

			return result;
		}

		protected virtual bool ValidateWeaponsDice()
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
				result = Record.GetWeapons(i).Dice >= 1 && Record.GetWeapons(i).Dice <= 25;
			}
			else
			{
				result = Record.GetWeapons(i).Dice == 0;
			}

			return result;
		}

		protected virtual bool ValidateWeaponsSides()
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
				result = Record.GetWeapons(i).Sides >= 1 && Record.GetWeapons(i).Sides <= 25;
			}
			else
			{
				result = Record.GetWeapons(i).Sides == 0;
			}

			return result;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		protected virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the character.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescGender()
		{
			var fullDesc = "Enter the gender of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var genderValues = EnumUtil.GetValues<Enums.Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], Globals.Engine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescStatus()
		{
			var fullDesc = "Enter the status of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var statusValues = EnumUtil.GetValues<Enums.Status>();

			for (var j = 0; j < statusValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)statusValues[j], Globals.Engine.GetStatusNames(statusValues[j]));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			var fullDesc = string.Format("Enter the {0} of the character.", stat.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", stat.MinValue, stat.MaxValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescSpellAbilitiesElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			var fullDesc = string.Format("Enter the character's {0} spell ability.", spell.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", spell.MinValue, spell.MaxValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			var fullDesc = string.Format("Enter the character's {0} weapon ability.", weapon.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", weapon.MinValue, weapon.MaxValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArmorExpertise()
		{
			var fullDesc = "Enter the armor expertise of the character.";

			var briefDesc = "0-79=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescHeldGold()
		{
			var fullDesc = "Enter the character's gold in hand.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescBankGold()
		{
			var fullDesc = "Enter the character's gold in the bank.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArmorClass()
		{
			var fullDesc = "Enter the armor class of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var armorValues = EnumUtil.GetValues<Enums.Armor>();

			for (var j = 0; j < armorValues.Count; j++)
			{
				var armor = Globals.Engine.GetArmors(armorValues[j]);

				Debug.Assert(armor != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescWeaponsName()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} name.", i + 1);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescWeaponsIsPlural()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} Is Plural status.", i + 1);

			var briefDesc = "0=Singular; 1=Plural";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsPluralType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} plural type.", i + 1);

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsArticleType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} article type.", i + 1);

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsComplexity()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} complexity.", i + 1);

			var briefDesc = "-50-50=Valid value";          // TODO: eliminate hardcode

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} type.", i + 1);

			var briefDesc = new StringBuilder(Constants.BufSize);

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			for (var j = 0; j < weaponValues.Count; j++)
			{
				var weapon = Globals.Engine.GetWeapons(weaponValues[j]);

				Debug.Assert(weapon != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescWeaponsDice()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} hit dice.", i + 1);

			var briefDesc = "1-25=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsSides()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the character's weapon #{0} hit dice sides.", i + 1);

			var briefDesc = "1-25=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		#endregion

		#region List Methods

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

		protected virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		protected virtual void ListGender()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null), Record.EvalGender("Male", "Female", "Neutral"));
			}
		}

		protected virtual void ListStatus()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Status"), null), Globals.Engine.GetStatusNames(Record.Status));
			}
		}

		protected virtual void ListStats()
		{
			var statValues = EnumUtil.GetValues<Enums.Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				ListField("StatsElement");
			}

			AddToListedNames = false;
		}

		protected virtual void ListStatsElement()
		{
			var i = Index;

			var sv = (Enums.Stat)i;

			if (FullDetail)
			{
				if (LookupMsg)
				{
					Buf.Clear();

					if (sv == Enums.Stat.Intellect)
					{
						var ibp = Record.GetIntellectBonusPct();

						Buf.AppendFormat("Learning: {0}{1}%", ibp > 0 ? "+" : "", ibp);
					}
					else if (sv == Enums.Stat.Hardiness)
					{
						Buf.AppendFormat("Weight Carryable: {0} G ({1} D)", Record.GetWeightCarryableGronds(), Record.GetWeightCarryableDos());
					}
					else if (sv == Enums.Stat.Charisma)
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

		protected virtual void ListSpellAbilities()
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				ListField("SpellAbilitiesElement");
			}

			AddToListedNames = false;
		}

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

		protected virtual void ListWeaponAbilities()
		{
			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				ListField("WeaponAbilitiesElement");
			}

			AddToListedNames = false;
		}

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

		protected virtual void ListArmorExpertise()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArmorExpertise"), null), Record.ArmorExpertise);
			}
		}

		protected virtual void ListHeldGold()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("HeldGold"), null), Record.HeldGold);
			}
		}

		protected virtual void ListBankGold()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("BankGold"), null), Record.BankGold);
			}
		}

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

		protected virtual void ListWeapons()
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				ListField("WeaponsName");
				ListField("WeaponsIsPlural");
				ListField("WeaponsPluralType");
				ListField("WeaponsArticleType");
				ListField("WeaponsComplexity");
				ListField("WeaponsType");
				ListField("WeaponsDice");
				ListField("WeaponsSides");
			}

			AddToListedNames = false;
		}

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
							Record.GetWeapons(i).PluralType == Enums.PluralType.None ? "No change" :
							Record.GetWeapons(i).PluralType == Enums.PluralType.S ? "Use 's'" :
							Record.GetWeapons(i).PluralType == Enums.PluralType.Es ? "Use 'es'" :
							Record.GetWeapons(i).PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsPluralType"), null), (long)Record.GetWeapons(i).PluralType);
					}
				}
			}
		}

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
							Record.GetWeapons(i).ArticleType == Enums.ArticleType.None ? "No article" :
							Record.GetWeapons(i).ArticleType == Enums.ArticleType.A ? "Use 'a'" :
							Record.GetWeapons(i).ArticleType == Enums.ArticleType.An ? "Use 'an'" :
							Record.GetWeapons(i).ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
							Record.GetWeapons(i).ArticleType == Enums.ArticleType.The ? "Use 'the'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsArticleType"), null), (long)Record.GetWeapons(i).ArticleType);
					}
				}
			}
		}

		protected virtual void ListWeaponsComplexity()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsComplexity"), null), Record.GetWeapons(i).Complexity);
				}
			}
		}

		protected virtual void ListWeaponsType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var weapon = Globals.Engine.GetWeapons(Record.GetWeapons(i).Type);

					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsType"), null),
						weapon != null ? weapon.Name : "0");
				}
			}
		}

		protected virtual void ListWeaponsDice()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsDice"), null), Record.GetWeapons(i).Dice);
				}
			}
		}

		protected virtual void ListWeaponsSides()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsSides"), null), Record.GetWeapons(i).Sides);
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				Record.Name = Buf.Trim().ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				Record.Gender = (Enums.Gender)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Gender"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				Record.Status = (Enums.Status)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Status"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputStats()
		{
			var statValues = EnumUtil.GetValues<Enums.Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				InputField("StatsElement");
			}
		}

		protected virtual void InputStatsElement()
		{
			var i = Index;

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

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

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputSpellAbilities()
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				InputField("SpellAbilitiesElement");
			}
		}

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

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWeaponAbilities()
		{
			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				InputField("WeaponAbilitiesElement");
			}
		}

		protected virtual void InputWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

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

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				Record.ArmorClass = (Enums.Armor)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArmorClass"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWeapons()
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				InputField("WeaponsName");
				InputField("WeaponsIsPlural");
				InputField("WeaponsPluralType");
				InputField("WeaponsArticleType");
				InputField("WeaponsComplexity");
				InputField("WeaponsType");
				InputField("WeaponsDice");
				InputField("WeaponsSides");
			}
		}

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

					var rc = Globals.In.ReadField(Buf, Constants.CharWpnNameLen, null, '_', '\0', true, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Name = Buf.Trim().ToString();

					if (ValidateField("WeaponsName"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				if (Record.IsWeaponActive(i))
				{
					if (EditRec && (Record.GetWeapons(i).Dice == 0 || Record.GetWeapons(i).Sides == 0))
					{
						Record.GetWeapons(i).IsPlural = false;

						Record.GetWeapons(i).PluralType = Enums.PluralType.S;

						Record.GetWeapons(i).ArticleType = Enums.ArticleType.A;

						Record.GetWeapons(i).Complexity = 5;

						Record.GetWeapons(i).Type = Enums.Weapon.Sword;

						Record.GetWeapons(i).Dice = 1;

						Record.GetWeapons(i).Sides = 6;
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

						Record.GetWeapons(k).Complexity = 0;

						Record.GetWeapons(k).Type = 0;

						Record.GetWeapons(k).Dice = 0;

						Record.GetWeapons(k).Sides = 0;
					}
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Name = "NONE";
			}
		}

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

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).IsPlural = false;
			}
		}

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
						Record.GetWeapons(i).PluralType = (Enums.PluralType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsPluralType"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).PluralType = 0;
			}
		}

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
						Record.GetWeapons(i).ArticleType = (Enums.ArticleType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsArticleType"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).ArticleType = 0;
			}
		}

		protected virtual void InputWeaponsComplexity()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var complexity = Record.GetWeapons(i).Complexity;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", complexity);

					PrintFieldDesc("WeaponsComplexity", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsComplexity"), "5"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).Complexity = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsComplexity"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Complexity = 0;
			}
		}

		protected virtual void InputWeaponsType()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var type = Record.GetWeapons(i).Type;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc("WeaponsType", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsType"), "5"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Type = (Enums.Weapon)Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsType"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Type = 0;
			}
		}

		protected virtual void InputWeaponsDice()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var dice = Record.GetWeapons(i).Dice;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", dice);

					PrintFieldDesc("WeaponsDice", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsDice"), "1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Dice = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsDice"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Dice = 0;
			}
		}

		protected virtual void InputWeaponsSides()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var sides = Record.GetWeapons(i).Sides;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", sides);

					PrintFieldDesc("WeaponsSides", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsSides"), "6"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Sides = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsSides"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetWeapons(i).Sides = 0;
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

		// do nothing

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
				"Weapons",
			};
		}

		#endregion

		#endregion
	}
}
