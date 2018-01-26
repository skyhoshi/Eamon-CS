
// CharacterHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<ICharacter>))]
	public class CharacterHelper : Helper<ICharacter>
	{
		#region Protected Methods

		#region Interface IHelper

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.CharNameLen;
		}

		protected virtual bool ValidateArticleType(IField field, IValidateArgs args)
		{
			return Record.ArticleType == Enums.ArticleType.None;
		}

		protected virtual bool ValidateGender(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Gender), Record.Gender);
		}

		protected virtual bool ValidateStatus(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Status), Record.Status);
		}

		protected virtual bool ValidateStats(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			return Record.GetStats(i) >= stat.MinValue && Record.GetStats(i) <= stat.MaxValue;
		}

		protected virtual bool ValidateSpellAbilities(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSpellAbilities(i) >= spell.MinValue && Record.GetSpellAbilities(i) <= spell.MaxValue;
		}

		protected virtual bool ValidateWeaponAbilities(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			return Record.GetWeaponAbilities(i) >= weapon.MinValue && Record.GetWeaponAbilities(i) <= weapon.MaxValue;
		}

		protected virtual bool ValidateArmorExpertise(IField field, IValidateArgs args)
		{
			return Record.ArmorExpertise >= 0 && Record.ArmorExpertise <= 79;
		}

		protected virtual bool ValidateHeldGold(IField field, IValidateArgs args)
		{
			return Record.HeldGold >= Constants.MinGoldValue && Record.HeldGold <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateBankGold(IField field, IValidateArgs args)
		{
			return Record.BankGold >= Constants.MinGoldValue && Record.BankGold <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateArmorClass(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Armor), Record.ArmorClass);
		}

		protected virtual bool ValidateWeaponsName(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsIsPlural(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsPluralType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsArticleType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsComplexity(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsDice(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		protected virtual bool ValidateWeaponsSides(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

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

		#region PrintFieldDesc Methods

		protected virtual void PrintDescName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the name of the character.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescGender(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the gender of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var genderValues = EnumUtil.GetValues<Enums.Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], Globals.Engine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescStatus(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the status of the character.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var statusValues = EnumUtil.GetValues<Enums.Status>();

			for (var j = 0; j < statusValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)statusValues[j], Globals.Engine.GetStatusNames(statusValues[j]));
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescStats(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			var fullDesc = string.Format("Enter the {0} of the character.", stat.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", stat.MinValue, stat.MaxValue);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescSpellAbilities(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			var fullDesc = string.Format("Enter the character's {0} spell ability.", spell.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", spell.MinValue, spell.MaxValue);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponAbilities(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			var fullDesc = string.Format("Enter the character's {0} weapon ability.", weapon.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", weapon.MinValue, weapon.MaxValue);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArmorExpertise(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the armor expertise of the character.";

			var briefDesc = "0-79=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescHeldGold(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the character's gold in hand.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescBankGold(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the character's gold in the bank.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArmorClass(IField field, IPrintDescArgs args)
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

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescWeaponsName(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} name.", i + 1);

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescWeaponsIsPlural(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} Is Plural status.", i + 1);

			var briefDesc = "0=Singular; 1=Plural";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsPluralType(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} plural type.", i + 1);

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsArticleType(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} article type.", i + 1);

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsComplexity(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} complexity.", i + 1);

			var briefDesc = "-50-50=Valid value";          // TODO: eliminate hardcode

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsType(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} type.", i + 1);

			var briefDesc = new StringBuilder(Constants.BufSize);

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			for (var j = 0; j < weaponValues.Count; j++)
			{
				var weapon = Globals.Engine.GetWeapons(weaponValues[j]);

				Debug.Assert(weapon != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescWeaponsDice(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} hit dice.", i + 1);

			var briefDesc = "1-25=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeaponsSides(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the character's weapon #{0} hit dice sides.", i + 1);

			var briefDesc = "1-25=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		#endregion

		#region List Methods

		protected virtual void ListUid(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Globals.Engine.Capitalize(Record.Name));
			}
		}

		protected virtual void ListName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Name);
			}
		}

		protected virtual void ListGender(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.EvalGender("Male", "Female", "Neutral"));
			}
		}

		protected virtual void ListStatus(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Globals.Engine.GetStatusNames(Record.Status));
			}
		}

		protected virtual void ListStats(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				var sv = (Enums.Stat)i;

				if (args.LookupMsg)
				{
					args.Buf.Clear();

					if (sv == Enums.Stat.Intellect)
					{
						var ibp = Record.GetIntellectBonusPct();

						args.Buf.AppendFormat("Learning: {0}{1}%", ibp > 0 ? "+" : "", ibp);
					}
					else if (sv == Enums.Stat.Hardiness)
					{
						args.Buf.AppendFormat("Weight Carryable: {0} G ({1} D)", Record.GetWeightCarryableGronds(), Record.GetWeightCarryableDos());
					}
					else if (sv == Enums.Stat.Charisma)
					{
						var cmp = Record.GetCharmMonsterPct();

						args.Buf.AppendFormat("Charm Monster: {0}{1}%", cmp > 0 ? "+" : "", cmp);
					}
				}

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg && args.Buf.Length > 0)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.GetStats(i), null, args.Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Record.GetStats(i));
				}
			}
		}

		protected virtual void ListSpellAbilities(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}%",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.GetSpellAbilities(i));
			}
		}

		protected virtual void ListWeaponAbilities(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}%",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.GetWeaponAbilities(i));
			}
		}

		protected virtual void ListArmorExpertise(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.ArmorExpertise);
			}
		}

		protected virtual void ListHeldGold(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.HeldGold);
			}
		}

		protected virtual void ListBankGold(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.BankGold);
			}
		}

		protected virtual void ListArmorClass(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				var armor = Globals.Engine.GetArmors(Record.ArmorClass);

				Debug.Assert(armor != null);

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), armor.Name);
			}
		}

		protected virtual void ListWeaponsName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || i == 0 || Record.IsWeaponActive(i - 1))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetWeapons(i).Name);
				}
			}
		}

		protected virtual void ListWeaponsIsPlural(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.GetWeapons(i).IsPlural));
				}
			}
		}

		protected virtual void ListWeaponsPluralType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.GetWeapons(i).PluralType, null,
							Record.GetWeapons(i).PluralType == Enums.PluralType.None ? "No change" :
							Record.GetWeapons(i).PluralType == Enums.PluralType.S ? "Use 's'" :
							Record.GetWeapons(i).PluralType == Enums.PluralType.Es ? "Use 'es'" :
							Record.GetWeapons(i).PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.GetWeapons(i).PluralType);
					}
				}
			}
		}

		protected virtual void ListWeaponsArticleType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
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
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.GetWeapons(i).ArticleType);
					}
				}
			}
		}

		protected virtual void ListWeaponsComplexity(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetWeapons(i).Complexity);
				}
			}
		}

		protected virtual void ListWeaponsType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					var weapon = Globals.Engine.GetWeapons(Record.GetWeapons(i).Type);

					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						weapon != null ? weapon.Name : "0");
				}
			}
		}

		protected virtual void ListWeaponsDice(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetWeapons(i).Dice);
				}
			}
		}

		protected virtual void ListWeaponsSides(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetWeapons(i).Sides);
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var name = Record.Name;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.CharNameLen, null, '_', '\0', false, null, null, Globals.Engine.IsCharAnyButDquoteCommaColon, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputGender(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var gender = Record.Gender;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Gender = (Enums.Gender)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputStatus(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var status = Record.Status;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)status);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0To3, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Status = (Enums.Status)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputStats(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			var fieldDesc = args.FieldDesc;

			var value = Record.GetStats(i);

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), stat.EmptyVal));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, stat.EmptyVal, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.SetStats(i, Convert.ToInt64(args.Buf.Trim().ToString()));

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputSpellAbilities(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var fieldDesc = args.FieldDesc;

			var value = Record.GetSpellAbilities(i);

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.SetSpellAbilities(i, Convert.ToInt64(args.Buf.Trim().ToString()));

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWeaponAbilities(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			var fieldDesc = args.FieldDesc;

			var value = Record.GetWeaponAbilities(i);

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), weapon.EmptyVal));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, weapon.EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.SetWeaponAbilities(i, Convert.ToInt64(args.Buf.Trim().ToString()));
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputArmorExpertise(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var armorExpertise = Record.ArmorExpertise;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", armorExpertise);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArmorExpertise = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputHeldGold(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var heldGold = Record.HeldGold;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", heldGold);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "200"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "200", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.HeldGold = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputBankGold(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var bankGold = Record.BankGold;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", bankGold);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.BankGold = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputArmorClass(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var armorClass = Record.ArmorClass;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)armorClass);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArmorClass = (Enums.Armor)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWeaponsName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (i == 0 || Record.IsWeaponActive(i - 1))
			{
				var fieldDesc = args.FieldDesc;

				var name = Record.GetWeapons(i).Name;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

					var rc = Globals.In.ReadField(args.Buf, Constants.CharWpnNameLen, null, '_', '\0', true, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Name = args.Buf.Trim().ToString();

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				if (Record.IsWeaponActive(i))
				{
					if (args.EditRec && (Record.GetWeapons(i).Dice == 0 || Record.GetWeapons(i).Sides == 0))
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

		protected virtual void InputWeaponsIsPlural(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var isPlural = Record.GetWeapons(i).IsPlural;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).IsPlural = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

					if (ValidateField(field, args.Vargs))
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

		protected virtual void InputWeaponsPluralType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var pluralType = Record.GetWeapons(i).PluralType;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)pluralType);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).PluralType = (Enums.PluralType)Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
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

		protected virtual void InputWeaponsArticleType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var articleType = Record.GetWeapons(i).ArticleType;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)articleType);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).ArticleType = (Enums.ArticleType)Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
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

		protected virtual void InputWeaponsComplexity(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var complexity = Record.GetWeapons(i).Complexity;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", complexity);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "5"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapons(i).Complexity = Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
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

		protected virtual void InputWeaponsType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var type = Record.GetWeapons(i).Type;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "5"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Type = (Enums.Weapon)Convert.ToInt64(args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
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

		protected virtual void InputWeaponsDice(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var dice = Record.GetWeapons(i).Dice;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", dice);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Dice = Convert.ToInt64(args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
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

		protected virtual void InputWeaponsSides(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var sides = Record.GetWeapons(i).Sides;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", sides);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "6"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Record.GetWeapons(i).Sides = Convert.ToInt64(args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
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

		protected override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Uid";
						x.Validate = ValidateUid;
						x.List = ListUid;
						x.Input = InputUid;
						x.GetPrintedName = () => "Uid";
						x.GetValue = () => Record.Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => Record.IsUidRecycled;
					}),
					GetNameField(),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Seen";
						x.GetPrintedName = () => "Seen";
						x.GetValue = () => Record.Seen;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArticleType";
						x.Validate = ValidateArticleType;
						x.GetPrintedName = () => "Article Type";
						x.GetValue = () => Record.ArticleType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Gender";
						x.Validate = ValidateGender;
						x.PrintDesc = PrintDescGender;
						x.List = ListGender;
						x.Input = InputGender;
						x.GetPrintedName = () => "Gender";
						x.GetValue = () => Record.Gender;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Status";
						x.Validate = ValidateStatus;
						x.PrintDesc = PrintDescStatus;
						x.List = ListStatus;
						x.Input = InputStatus;
						x.GetPrintedName = () => "Status";
						x.GetValue = () => Record.Status;
					})
				};

				var statValues = EnumUtil.GetValues<Enums.Stat>();

				foreach (var sv in statValues)
				{
					var i = (long)sv;

					var stat = Globals.Engine.GetStats(sv);

					Debug.Assert(stat != null);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Stats[{0}]", i);
							x.UserData = i;
							x.Validate = ValidateStats;
							x.PrintDesc = PrintDescStats;
							x.List = ListStats;
							x.Input = InputStats;
							x.GetPrintedName = () => stat.Name;
							x.GetValue = () => Record.GetStats(i);
						})
					);
				}

				var spellValues = EnumUtil.GetValues<Enums.Spell>();

				foreach (var sv in spellValues)
				{
					var i = (long)sv;

					var spell = Globals.Engine.GetSpells(sv);

					Debug.Assert(spell != null);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("SpellAbilities[{0}]", i);
							x.UserData = i;
							x.Validate = ValidateSpellAbilities;
							x.PrintDesc = PrintDescSpellAbilities;
							x.List = ListSpellAbilities;
							x.Input = InputSpellAbilities;
							x.GetPrintedName = () => string.Format("{0} Spell Ability", spell.Name);
							x.GetValue = () => Record.GetSpellAbilities(i);
						})
					);
				}

				var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

				foreach (var wv in weaponValues)
				{
					var i = (long)wv;

					var weapon = Globals.Engine.GetWeapons(wv);

					Debug.Assert(weapon != null);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("WeaponAbilities[{0}]", i);
							x.UserData = i;
							x.Validate = ValidateWeaponAbilities;
							x.PrintDesc = PrintDescWeaponAbilities;
							x.List = ListWeaponAbilities;
							x.Input = InputWeaponAbilities;
							x.GetPrintedName = () => string.Format("{0} Wpn Ability", weapon.Name);
							x.GetValue = () => Record.GetWeaponAbilities(i);
						})
					);
				}

				Fields.AddRange(new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArmorExpertise";
						x.Validate = ValidateArmorExpertise;
						x.PrintDesc = PrintDescArmorExpertise;
						x.List = ListArmorExpertise;
						x.Input = InputArmorExpertise;
						x.GetPrintedName = () => "Armor Expertise";
						x.GetValue = () => Record.ArmorExpertise;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "HeldGold";
						x.Validate = ValidateHeldGold;
						x.PrintDesc = PrintDescHeldGold;
						x.List = ListHeldGold;
						x.Input = InputHeldGold;
						x.GetPrintedName = () => "Held Gold";
						x.GetValue = () => Record.HeldGold;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "BankGold";
						x.Validate = ValidateBankGold;
						x.PrintDesc = PrintDescBankGold;
						x.List = ListBankGold;
						x.Input = InputBankGold;
						x.GetPrintedName = () => "Bank Gold";
						x.GetValue = () => Record.BankGold;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArmorClass";
						x.Validate = ValidateArmorClass;
						x.PrintDesc = PrintDescArmorClass;
						x.List = ListArmorClass;
						x.Input = InputArmorClass;
						x.GetPrintedName = () => "Armor Class";
						x.GetValue = () => Record.ArmorClass;
					})
				});

				for (var i = 0; i < Record.Weapons.Length; i++)
				{
					var j = i;

					Fields.AddRange(new List<IField>()
					{
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Name", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsName;
							x.PrintDesc = PrintDescWeaponsName;
							x.List = ListWeaponsName;
							x.Input = InputWeaponsName;
							x.GetPrintedName = () => string.Format("Wpn #{0} Name", j + 1);
							x.GetValue = () => Record.GetWeapons(j).Name;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].IsPlural", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsIsPlural;
							x.PrintDesc = PrintDescWeaponsIsPlural;
							x.List = ListWeaponsIsPlural;
							x.Input = InputWeaponsIsPlural;
							x.GetPrintedName = () => string.Format("Wpn #{0} Is Plural", j + 1);
							x.GetValue = () => Record.GetWeapons(j).IsPlural;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].PluralType", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsPluralType;
							x.PrintDesc = PrintDescWeaponsPluralType;
							x.List = ListWeaponsPluralType;
							x.Input = InputWeaponsPluralType;
							x.GetPrintedName = () => string.Format("Wpn #{0} Plural Type", j + 1);
							x.GetValue = () => Record.GetWeapons(j).PluralType;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].ArticleType", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsArticleType;
							x.PrintDesc = PrintDescWeaponsArticleType;
							x.List = ListWeaponsArticleType;
							x.Input = InputWeaponsArticleType;
							x.GetPrintedName = () => string.Format("Wpn #{0} Article Type", j + 1);
							x.GetValue = () => Record.GetWeapons(j).ArticleType;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Complexity", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsComplexity;
							x.PrintDesc = PrintDescWeaponsComplexity;
							x.List = ListWeaponsComplexity;
							x.Input = InputWeaponsComplexity;
							x.GetPrintedName = () => string.Format("Wpn #{0} Complexity", j + 1);
							x.GetValue = () => Record.GetWeapons(j).Complexity;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Type", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsType;
							x.PrintDesc = PrintDescWeaponsType;
							x.List = ListWeaponsType;
							x.Input = InputWeaponsType;
							x.GetPrintedName = () => string.Format("Wpn #{0} Type", j + 1);
							x.GetValue = () => Record.GetWeapons(j).Type;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Dice", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsDice;
							x.PrintDesc = PrintDescWeaponsDice;
							x.List = ListWeaponsDice;
							x.Input = InputWeaponsDice;
							x.GetPrintedName = () => string.Format("Wpn #{0} Dice", j + 1);
							x.GetValue = () => Record.GetWeapons(j).Dice;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Sides", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsSides;
							x.PrintDesc = PrintDescWeaponsSides;
							x.List = ListWeaponsSides;
							x.Input = InputWeaponsSides;
							x.GetPrintedName = () => string.Format("Wpn #{0} Sides", j + 1);
							x.GetValue = () => Record.GetWeapons(j).Sides;
						})
					});
				}
			}

			return Fields;
		}

		protected override IField GetNameField()
		{
			if (NameField == null)
			{
				NameField = Globals.CreateInstance<IField>(x =>
				{
					x.Name = "Name";
					x.GetPrintedName = () => "Name";
					x.Validate = ValidateName;
					x.PrintDesc = PrintDescName;
					x.List = ListName;
					x.Input = InputName;
					x.BuildValue = null;
					x.GetValue = () => Record.Name;
				});
			}

			return NameField;
		}

		#endregion

		#region Class CharacterHelper

		protected virtual void SetCharacterUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetCharacterUid();

				Record.IsUidRecycled = true;
			}
			else if (!editRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Class CharacterHelper

		public CharacterHelper()
		{
			SetUidIfInvalid = SetCharacterUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
