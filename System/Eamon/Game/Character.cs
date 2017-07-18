
// Character.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.DataEntry;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Character : Editable, ICharacter
	{
		#region Protected Properties

		[ExcludeFromSerialization]
		protected virtual IField NameField { get; set; }

		#endregion

		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

		#region Interface IHaveListedName

		public virtual string Name { get; set; }

		public virtual string[] Synonyms { get; set; }

		public virtual bool Seen { get; set; }

		public virtual Enums.ArticleType ArticleType { get; set; }

		#endregion

		#region Interface ICharacter

		public virtual Enums.Gender Gender { get; set; }

		public virtual Enums.Status Status { get; set; }

		public virtual long[] Stats { get; set; }

		public virtual long[] SpellAbilities { get; set; }

		public virtual long[] WeaponAbilities { get; set; }

		public virtual long ArmorExpertise { get; set; }

		public virtual long HeldGold { get; set; }

		public virtual long BankGold { get; set; }

		public virtual Enums.Armor ArmorClass { get; set; }

		public virtual Classes.ICharacterWeapon[] Weapons { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeCharacterUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IValidator

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Uid > 0;
		}

		protected virtual bool ValidateName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Name) == false && Name.Length <= Constants.CharNameLen;
		}

		protected virtual bool ValidateArticleType(IField field, IValidateArgs args)
		{
			return ArticleType == Enums.ArticleType.None;
		}

		protected virtual bool ValidateGender(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Gender), Gender);
		}

		protected virtual bool ValidateStatus(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Status), Status);
		}

		protected virtual bool ValidateStats(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			return GetStats(i) >= stat.MinValue && GetStats(i) <= stat.MaxValue;
		}

		protected virtual bool ValidateSpellAbilities(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return GetSpellAbilities(i) >= spell.MinValue && GetSpellAbilities(i) <= spell.MaxValue;
		}

		protected virtual bool ValidateWeaponAbilities(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			return GetWeaponAbilities(i) >= weapon.MinValue && GetWeaponAbilities(i) <= weapon.MaxValue;
		}

		protected virtual bool ValidateArmorExpertise(IField field, IValidateArgs args)
		{
			return ArmorExpertise >= 0 && ArmorExpertise <= 79;
		}

		protected virtual bool ValidateHeldGold(IField field, IValidateArgs args)
		{
			return HeldGold >= Constants.MinGoldValue && HeldGold <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateBankGold(IField field, IValidateArgs args)
		{
			return BankGold >= Constants.MinGoldValue && BankGold <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateArmorClass(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Armor), ArmorClass);
		}

		protected virtual bool ValidateWeaponsName(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeWeapon = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 0; h <= i; h++)
			{
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				if (GetWeapons(i).Name.Length > Constants.CharWpnNameLen)
				{
					for (var j = Constants.CharWpnNameLen; j < GetWeapons(i).Name.Length; j++)
					{
						if (GetWeapons(i).Name[j] != '#')
						{
							result = false;

							break;
						}
					}
				}
			}
			else
			{
				result = GetWeapons(i).Name != null && (GetWeapons(i).Name == "" || string.Equals(GetWeapons(i).Name, "NONE", StringComparison.OrdinalIgnoreCase));
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (!activeWeapon)
			{
				result = GetWeapons(i).IsPlural == false;
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(Enums.PluralType), GetWeapons(i).PluralType);
			}
			else
			{
				result = GetWeapons(i).PluralType == Enums.PluralType.None;
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(Enums.ArticleType), GetWeapons(i).ArticleType);
			}
			else
			{
				result = GetWeapons(i).ArticleType == Enums.ArticleType.None;
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = GetWeapons(i).Complexity >= -50 && GetWeapons(i).Complexity <= 50;
			}
			else
			{
				result = GetWeapons(i).Complexity == 0;
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(Enums.Weapon), GetWeapons(i).Type);
			}
			else
			{
				result = GetWeapons(i).Type == 0;
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = GetWeapons(i).Dice >= 1 && GetWeapons(i).Dice <= 25;
			}
			else
			{
				result = GetWeapons(i).Dice == 0;
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
				if (!IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = GetWeapons(i).Sides >= 1 && GetWeapons(i).Sides <= 25;
			}
			else
			{
				result = GetWeapons(i).Sides == 0;
			}

			return result;
		}

		#endregion

		#region Interface IEditable

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
				var gender = Globals.Engine.GetGenders(genderValues[j]);

				Debug.Assert(gender != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], gender.Name);
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

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Uid, Globals.Engine.Capitalize(Name));
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Name);
			}
		}

		protected virtual void ListGender(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				var gender = Globals.Engine.GetGenders(Gender);

				Debug.Assert(gender != null);

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), gender.Name);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Globals.Engine.GetStatusNames(Status));
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
						var ibp = GetIntellectBonusPct();

						args.Buf.AppendFormat("Learning: {0}{1}%", ibp > 0 ? "+" : "", ibp);
					}
					else if (sv == Enums.Stat.Hardiness)
					{
						args.Buf.AppendFormat("Weight Carryable: {0} G ({1} D)", GetWeightCarryableGronds(), GetWeightCarryableDos());
					}
					else if (sv == Enums.Stat.Charisma)
					{
						var cmp = GetCharmMonsterPct();

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
						Globals.Engine.BuildValue(51, ' ', 8, GetStats(i), null, args.Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						GetStats(i));
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
					GetSpellAbilities(i));
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
					GetWeaponAbilities(i));
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

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), ArmorExpertise);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), HeldGold);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), BankGold);
			}
		}

		protected virtual void ListArmorClass(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				var armor = Globals.Engine.GetArmors(ArmorClass);

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
				if (!args.ExcludeROFields || i == 0 || IsWeaponActive(i - 1))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetWeapons(i).Name);
				}
			}
		}

		protected virtual void ListWeaponsIsPlural(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(GetWeapons(i).IsPlural));
				}
			}
		}

		protected virtual void ListWeaponsPluralType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || IsWeaponActive(i))
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
							Globals.Engine.BuildValue(51, ' ', 8, (long)GetWeapons(i).PluralType, null,
							GetWeapons(i).PluralType == Enums.PluralType.None ? "No change" :
							GetWeapons(i).PluralType == Enums.PluralType.S ? "Use 's'" :
							GetWeapons(i).PluralType == Enums.PluralType.Es ? "Use 'es'" :
							GetWeapons(i).PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)GetWeapons(i).PluralType);
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
				if (!args.ExcludeROFields || IsWeaponActive(i))
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
							Globals.Engine.BuildValue(51, ' ', 8, (long)GetWeapons(i).ArticleType, null,
							GetWeapons(i).ArticleType == Enums.ArticleType.None ? "No article" :
							GetWeapons(i).ArticleType == Enums.ArticleType.A ? "Use 'a'" :
							GetWeapons(i).ArticleType == Enums.ArticleType.An ? "Use 'an'" :
							GetWeapons(i).ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
							GetWeapons(i).ArticleType == Enums.ArticleType.The ? "Use 'the'" :
							"Invalid value"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)GetWeapons(i).ArticleType);
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
				if (!args.ExcludeROFields || IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetWeapons(i).Complexity);
				}
			}
		}

		protected virtual void ListWeaponsType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || IsWeaponActive(i))
				{
					var weapon = Globals.Engine.GetWeapons(GetWeapons(i).Type);

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
				if (!args.ExcludeROFields || IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetWeapons(i).Dice);
				}
			}
		}

		protected virtual void ListWeaponsSides(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || IsWeaponActive(i))
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetWeapons(i).Sides);
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var name = Name;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.CharNameLen, null, '_', '\0', false, null, null, Globals.Engine.IsCharAnyButDquoteCommaColon, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputGender(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var gender = Gender;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Gender = (Enums.Gender)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputStatus(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var status = Status;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)status);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0To3, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Status = (Enums.Status)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputStats(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			var fieldDesc = args.FieldDesc;

			var value = GetStats(i);

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), stat.EmptyVal));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, stat.EmptyVal, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				SetStats(i, Convert.ToInt64(args.Buf.Trim().ToString()));

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputSpellAbilities(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var fieldDesc = args.FieldDesc;

			var value = GetSpellAbilities(i);

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				SetSpellAbilities(i, Convert.ToInt64(args.Buf.Trim().ToString()));

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputWeaponAbilities(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			var fieldDesc = args.FieldDesc;

			var value = GetWeaponAbilities(i);

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
					SetWeaponAbilities(i, Convert.ToInt64(args.Buf.Trim().ToString()));
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

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputArmorExpertise(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var armorExpertise = ArmorExpertise;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", armorExpertise);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ArmorExpertise = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputHeldGold(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var heldGold = HeldGold;

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
					HeldGold = Convert.ToInt64(args.Buf.Trim().ToString());
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

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputBankGold(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var bankGold = BankGold;

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
					BankGold = Convert.ToInt64(args.Buf.Trim().ToString());
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

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputArmorClass(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var armorClass = ArmorClass;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)armorClass);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ArmorClass = (Enums.Armor)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputWeaponsName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (i == 0 || IsWeaponActive(i - 1))
			{
				var fieldDesc = args.FieldDesc;

				var name = GetWeapons(i).Name;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

					var rc = Globals.In.ReadField(args.Buf, Constants.CharWpnNameLen, null, '_', '\0', true, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					GetWeapons(i).Name = args.Buf.Trim().ToString();

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				if (IsWeaponActive(i))
				{
					if (args.EditRec && (GetWeapons(i).Dice == 0 || GetWeapons(i).Sides == 0))
					{
						GetWeapons(i).IsPlural = false;

						GetWeapons(i).PluralType = Enums.PluralType.S;

						GetWeapons(i).ArticleType = Enums.ArticleType.A;

						GetWeapons(i).Complexity = 5;

						GetWeapons(i).Type = Enums.Weapon.Sword;

						GetWeapons(i).Dice = 1;

						GetWeapons(i).Sides = 6;
					}
				}
				else
				{
					for (var k = i; k < Weapons.Length; k++)
					{
						GetWeapons(k).Name = "NONE";

						GetWeapons(k).IsPlural = false;

						GetWeapons(k).PluralType = 0;

						GetWeapons(k).ArticleType = 0;

						GetWeapons(k).Complexity = 0;

						GetWeapons(k).Type = 0;

						GetWeapons(k).Dice = 0;

						GetWeapons(k).Sides = 0;
					}
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).Name = "NONE";
			}
		}

		protected virtual void InputWeaponsIsPlural(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var isPlural = GetWeapons(i).IsPlural;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					GetWeapons(i).IsPlural = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).IsPlural = false;
			}
		}

		protected virtual void InputWeaponsPluralType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var pluralType = GetWeapons(i).PluralType;

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
						GetWeapons(i).PluralType = (Enums.PluralType)Convert.ToInt64(args.Buf.Trim().ToString());
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

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).PluralType = 0;
			}
		}

		protected virtual void InputWeaponsArticleType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var articleType = GetWeapons(i).ArticleType;

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
						GetWeapons(i).ArticleType = (Enums.ArticleType)Convert.ToInt64(args.Buf.Trim().ToString());
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

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).ArticleType = 0;
			}
		}

		protected virtual void InputWeaponsComplexity(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var complexity = GetWeapons(i).Complexity;

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
						GetWeapons(i).Complexity = Convert.ToInt64(args.Buf.Trim().ToString());
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

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).Complexity = 0;
			}
		}

		protected virtual void InputWeaponsType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var type = GetWeapons(i).Type;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "5"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "5", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					GetWeapons(i).Type = (Enums.Weapon)Convert.ToInt64(args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).Type = 0;
			}
		}

		protected virtual void InputWeaponsDice(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var dice = GetWeapons(i).Dice;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", dice);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					GetWeapons(i).Dice = Convert.ToInt64(args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).Dice = 0;
			}
		}

		protected virtual void InputWeaponsSides(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (IsWeaponActive(i))
			{
				var fieldDesc = args.FieldDesc;

				var sides = GetWeapons(i).Sides;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", sides);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "6"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					GetWeapons(i).Sides = Convert.ToInt64(args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetWeapons(i).Sides = 0;
			}
		}

		#endregion

		#endregion

		#region Class Character

		protected virtual void SetCharacterUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetCharacterUid();

				IsUidRecycled = true;
			}
			else if (!editRec)
			{
				IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

		public override void FreeFields()
		{
			base.FreeFields();

			NameField = null;
		}

		public override IList<IField> GetFields()
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
						x.GetValue = () => Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => IsUidRecycled;
					}),
					GetNameField(),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Seen";
						x.GetPrintedName = () => "Seen";
						x.GetValue = () => Seen;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArticleType";
						x.Validate = ValidateArticleType;
						x.GetPrintedName = () => "Article Type";
						x.GetValue = () => ArticleType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Gender";
						x.Validate = ValidateGender;
						x.PrintDesc = PrintDescGender;
						x.List = ListGender;
						x.Input = InputGender;
						x.GetPrintedName = () => "Gender";
						x.GetValue = () => Gender;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Status";
						x.Validate = ValidateStatus;
						x.PrintDesc = PrintDescStatus;
						x.List = ListStatus;
						x.Input = InputStatus;
						x.GetPrintedName = () => "Status";
						x.GetValue = () => Status;
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
							x.GetValue = () => GetStats(i);
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
							x.GetValue = () => GetSpellAbilities(i);
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
							x.GetValue = () => GetWeaponAbilities(i);
						})
					);
				}

				Fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArmorExpertise";
						x.Validate = ValidateArmorExpertise;
						x.PrintDesc = PrintDescArmorExpertise;
						x.List = ListArmorExpertise;
						x.Input = InputArmorExpertise;
						x.GetPrintedName = () => "Armor Expertise";
						x.GetValue = () => ArmorExpertise;
					})
				);

				Fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "HeldGold";
						x.Validate = ValidateHeldGold;
						x.PrintDesc = PrintDescHeldGold;
						x.List = ListHeldGold;
						x.Input = InputHeldGold;
						x.GetPrintedName = () => "Held Gold";
						x.GetValue = () => HeldGold;
					})
				);

				Fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "BankGold";
						x.Validate = ValidateBankGold;
						x.PrintDesc = PrintDescBankGold;
						x.List = ListBankGold;
						x.Input = InputBankGold;
						x.GetPrintedName = () => "Bank Gold";
						x.GetValue = () => BankGold;
					})
				);

				Fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArmorClass";
						x.Validate = ValidateArmorClass;
						x.PrintDesc = PrintDescArmorClass;
						x.List = ListArmorClass;
						x.Input = InputArmorClass;
						x.GetPrintedName = () => "Armor Class";
						x.GetValue = () => ArmorClass;
					})
				);

				for (var i = 0; i < Weapons.Length; i++)
				{
					var j = i;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Name", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsName;
							x.PrintDesc = PrintDescWeaponsName;
							x.List = ListWeaponsName;
							x.Input = InputWeaponsName;
							x.GetPrintedName = () => string.Format("Wpn #{0} Name", j + 1);
							x.GetValue = () => GetWeapons(j).Name;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].IsPlural", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsIsPlural;
							x.PrintDesc = PrintDescWeaponsIsPlural;
							x.List = ListWeaponsIsPlural;
							x.Input = InputWeaponsIsPlural;
							x.GetPrintedName = () => string.Format("Wpn #{0} Is Plural", j + 1);
							x.GetValue = () => GetWeapons(j).IsPlural;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].PluralType", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsPluralType;
							x.PrintDesc = PrintDescWeaponsPluralType;
							x.List = ListWeaponsPluralType;
							x.Input = InputWeaponsPluralType;
							x.GetPrintedName = () => string.Format("Wpn #{0} Plural Type", j + 1);
							x.GetValue = () => GetWeapons(j).PluralType;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].ArticleType", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsArticleType;
							x.PrintDesc = PrintDescWeaponsArticleType;
							x.List = ListWeaponsArticleType;
							x.Input = InputWeaponsArticleType;
							x.GetPrintedName = () => string.Format("Wpn #{0} Article Type", j + 1);
							x.GetValue = () => GetWeapons(j).ArticleType;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Complexity", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsComplexity;
							x.PrintDesc = PrintDescWeaponsComplexity;
							x.List = ListWeaponsComplexity;
							x.Input = InputWeaponsComplexity;
							x.GetPrintedName = () => string.Format("Wpn #{0} Complexity", j + 1);
							x.GetValue = () => GetWeapons(j).Complexity;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Type", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsType;
							x.PrintDesc = PrintDescWeaponsType;
							x.List = ListWeaponsType;
							x.Input = InputWeaponsType;
							x.GetPrintedName = () => string.Format("Wpn #{0} Type", j + 1);
							x.GetValue = () => GetWeapons(j).Type;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Dice", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsDice;
							x.PrintDesc = PrintDescWeaponsDice;
							x.List = ListWeaponsDice;
							x.Input = InputWeaponsDice;
							x.GetPrintedName = () => string.Format("Wpn #{0} Dice", j + 1);
							x.GetValue = () => GetWeapons(j).Dice;
						})
					);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Weapons[{0}].Sides", j);
							x.UserData = j;
							x.Validate = ValidateWeaponsSides;
							x.PrintDesc = PrintDescWeaponsSides;
							x.List = ListWeaponsSides;
							x.Input = InputWeaponsSides;
							x.GetPrintedName = () => string.Format("Wpn #{0} Sides", j + 1);
							x.GetValue = () => GetWeapons(j).Sides;
						})
					);
				}
			}

			return Fields;
		}

		#endregion

		#region Interface IHaveChildren

		public virtual void SetParentReferences()
		{
			foreach (var w in Weapons)
			{
				w.Parent = this;
			}
		}

		#endregion

		#region Interface IHaveListedName

		public virtual string GetPluralName(IField field, StringBuilder buf)
		{
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			buf.Append(Name);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetPluralName01(StringBuilder buf)
		{
			return GetPluralName(GetField("Name"), buf);
		}

		public virtual string GetDecoratedName(IField field, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			buf.Append(Name);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.None, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), ArticleType, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.The, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var name = GetDecoratedName02(true, true, false, false, new StringBuilder(Constants.BufSize));

			if (showName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					name);
			}

			var gender = Globals.Engine.GetGenders(Gender);

			Debug.Assert(gender != null);

			buf.AppendFormat("{0}You are the {1} {2}.{0}",
				Environment.NewLine,
				gender.MightyDesc,
				name);

		Cleanup:

			return rc;
		}

		public virtual IField GetNameField()
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
					x.GetValue = () => Name;
				});
			}

			return NameField;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(ICharacter character)
		{
			return this.Uid.CompareTo(character.Uid);
		}

		#endregion

		#region Interface ICharacter

		public virtual long GetStats(long index)
		{
			return Stats[index];
		}

		public virtual long GetStats(Enums.Stat stat)
		{
			return GetStats((long)stat);
		}

		public virtual long GetSpellAbilities(long index)
		{
			return SpellAbilities[index];
		}

		public virtual long GetSpellAbilities(Enums.Spell spell)
		{
			return GetSpellAbilities((long)spell);
		}

		public virtual long GetWeaponAbilities(long index)
		{
			return WeaponAbilities[index];
		}

		public virtual long GetWeaponAbilities(Enums.Weapon weapon)
		{
			return GetWeaponAbilities((long)weapon);
		}

		public virtual Classes.ICharacterWeapon GetWeapons(long index)
		{
			return Weapons[index];
		}

		public virtual string GetSynonyms(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetStats(long index, long value)
		{
			Stats[index] = value;
		}

		public virtual void SetStats(Enums.Stat stat, long value)
		{
			SetStats((long)stat, value);
		}

		public virtual void SetSpellAbilities(long index, long value)
		{
			SpellAbilities[index] = value;
		}

		public virtual void SetSpellAbilities(Enums.Spell spell, long value)
		{
			SetSpellAbilities((long)spell, value);
		}

		public virtual void SetWeaponAbilities(long index, long value)
		{
			WeaponAbilities[index] = value;
		}

		public virtual void SetWeaponAbilities(Enums.Weapon weapon, long value)
		{
			SetWeaponAbilities((long)weapon, value);
		}

		public virtual void SetWeapons(long index, Classes.ICharacterWeapon value)
		{
			Weapons[index] = value;
		}

		public virtual void SetSynonyms(long index, string value)
		{
			Synonyms[index] = value;
		}

		public virtual void ModStats(long index, long value)
		{
			Stats[index] += value;
		}

		public virtual void ModStats(Enums.Stat stat, long value)
		{
			ModStats((long)stat, value);
		}

		public virtual void ModSpellAbilities(long index, long value)
		{
			SpellAbilities[index] += value;
		}

		public virtual void ModSpellAbilities(Enums.Spell spell, long value)
		{
			ModSpellAbilities((long)spell, value);
		}

		public virtual void ModWeaponAbilities(long index, long value)
		{
			WeaponAbilities[index] += value;
		}

		public virtual void ModWeaponAbilities(Enums.Weapon weapon, long value)
		{
			ModWeaponAbilities((long)weapon, value);
		}

		public virtual long GetWeightCarryableGronds()
		{
			return Globals.Engine.GetWeightCarryableGronds(GetStats(Enums.Stat.Hardiness));
		}

		public virtual long GetWeightCarryableDos()
		{
			return Globals.Engine.GetWeightCarryableDos(GetStats(Enums.Stat.Hardiness));
		}

		public virtual long GetIntellectBonusPct()
		{
			return Globals.Engine.GetIntellectBonusPct(GetStats(Enums.Stat.Intellect));
		}

		public virtual long GetCharmMonsterPct()
		{
			return Globals.Engine.GetCharmMonsterPct(GetStats(Enums.Stat.Charisma));
		}

		public virtual long GetMerchantAdjustedCharisma()
		{
			return Globals.Engine.GetMerchantAdjustedCharisma(GetStats(Enums.Stat.Charisma));
		}

		public virtual bool IsWeaponActive(long index)
		{
			Debug.Assert(index >= 0 && index < Weapons.Length);

			return GetWeapons(index).IsActive();
		}

		public virtual T EvalGender<T>(T maleValue, T femaleValue, T neutralValue)
		{
			return Gender == Enums.Gender.Male ? maleValue : Gender == Enums.Gender.Female ? femaleValue : neutralValue;
		}

		public virtual RetCode GetBaseOddsToHit(Classes.ICharacterWeapon weapon, ref long baseOddsToHit)
		{
			long ar1, sh1, af, x, a, d, f, odds;
			RetCode rc;

			if (weapon == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (!weapon.IsActive())
			{
				baseOddsToHit = 0;

				goto Cleanup;
			}

			/* 
				Full Credit:  Derived wholly from Frank Black's Eamon Deluxe

				File: MAINHALL.BAS
				SUB	: Examine.Character
			*/

			/* COMPUTE ARMOR FACTOR */

			ar1 = (long)ArmorClass / 2;

			sh1 = (long)ArmorClass % 2;

			f = ar1;

			if (f > 3)
			{
				f = 3;
			}

			af = (-5 * sh1) - f * 10;

			if (f == 3)
			{
				af -= 30;
			}

			/* COMPUTE BASE ODDS TO HIT */

			x = GetStats(Enums.Stat.Agility);

			a = GetWeaponAbilities(weapon.Type);

			d = weapon.Complexity;

			if (x > 30)
			{
				x = 30;
			}

			if (a > 122)
			{
				a = 122;
			}

			if (d > 50)
			{
				d = 50;
			}

			odds = 50 + 2 * (x - (ar1 + sh1));

			odds = (long)Math.Round((double)odds + ((double)d / 2.0));

			odds += ((af + ArmorExpertise) * (-af > ArmorExpertise ? 1 : 0));

			odds = (long)Math.Round((double)odds + ((double)a / 4.0));

			/*
			if (odds > 100)
			{
				odds = 100;
			}
			*/

			baseOddsToHit = odds;

		Cleanup:

			return rc;
		}

		public virtual RetCode GetBaseOddsToHit(long index, ref long baseOddsToHit)
		{
			Debug.Assert(index >= 0 && index < Weapons.Length);

			return GetBaseOddsToHit(GetWeapons(index), ref baseOddsToHit);
		}

		public virtual RetCode GetWeaponCount(ref long count)
		{
			RetCode rc;
			long i;

			rc = RetCode.Success;

			for (i = 0; i < Weapons.Length; i++)
			{
				if (!IsWeaponActive(i))
				{
					break;
				}
			}

			count = i;

			return rc;
		}

		public virtual RetCode ListWeapons(StringBuilder buf, bool capitalize = true)
		{
			RetCode rc;
			long i;

			Classes.IWeapon weapon;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (i = 0; i < Weapons.Length; i++)
			{
				if (IsWeaponActive(i))
				{
					weapon = Globals.Engine.GetWeapons(GetWeapons(i).Type);

					Debug.Assert(weapon != null);

					buf.AppendFormat("{0}{1,2}. {2} ({3,-8}/{4,3}%/{5,2}D{6,-2})",
						Environment.NewLine,
						i + 1,
						capitalize ? Globals.Engine.Capitalize(GetWeapons(i).Name.PadTRight(31, ' ')) : GetWeapons(i).Name.PadTRight(31, ' '),
						weapon.Name,
						GetWeapons(i).Complexity,
						GetWeapons(i).Dice,
						GetWeapons(i).Sides);
				}
				else
				{
					break;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual void StripPoundCharsFromWeaponNames()
		{
			for (var i = 0; i < Weapons.Length; i++)
			{
				if (IsWeaponActive(i))
				{
					GetWeapons(i).Name = GetWeapons(i).Name.TrimEnd('#');
				}
			}
		}

		public virtual void AddPoundCharsToWeaponNames()
		{
			long c;

			do
			{
				c = 0;

				for (var i = 0; i < Weapons.Length; i++)
				{
					if (IsWeaponActive(i))
					{
						for (var j = i + 1; j < Weapons.Length; j++)
						{
							if (IsWeaponActive(j) && string.Equals(GetWeapons(j).Name, GetWeapons(i).Name, StringComparison.OrdinalIgnoreCase))
							{
								GetWeapons(j).Name += "#";

								c = 1;
							}
						}
					}
				}
			}
			while (c == 1);
		}

		public virtual RetCode StatDisplay(IStatDisplayArgs args)
		{
			StringBuilder buf01, buf02;
			RetCode rc;
			long i, j;

			Classes.IGender gender;
			Classes.IWeapon weapon;
			Classes.ISpell spell;

			if (args == null || args.Monster == null || args.ArmorString == null || args.SpellAbilities == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf01 = new StringBuilder(Constants.BufSize);

			buf02 = new StringBuilder(Constants.BufSize);

			gender = Globals.Engine.GetGenders(Gender);

			Debug.Assert(gender != null);

			Globals.Out.WriteLine("{0}{1,-36}Gender: {2,-9}Damage Taken: {3}/{4}",
				Environment.NewLine,
				args.Monster.Name.ToUpper(),
				gender.Name,
				args.Monster.DmgTaken,
				args.Monster.Hardiness);

			var ibp = Globals.Engine.GetIntellectBonusPct(GetStats(Enums.Stat.Intellect));

			buf01.AppendFormat("{0}{1}{2}%)",
				"(Learning: ",
				ibp > 0 ? "+" : "",
				ibp);

			buf02.AppendFormat("{0}{1}",
				args.Speed > 0 ? args.Monster.Agility / 2 : args.Monster.Agility,
				args.Speed > 0 ? "x2" : "");

			Globals.Out.WriteLine("{0}{1}{2,-2}{3,20}{4,15}{5}{0}{6}{7,-3}{8,34}{9,-2}{10,15}{11}{12}%)",
				Environment.NewLine,
				"Intellect:  ", GetStats(Enums.Stat.Intellect),
				buf01.ToString(),
				"Agility :  ", buf02.ToString(),
				"Hardiness:  ", args.Monster.Hardiness,
				"Charisma:  ", GetStats(Enums.Stat.Charisma),
				"(Charm Mon: ",
				args.CharmMon > 0 ? "+" : "",
				args.CharmMon);

			Globals.Out.Write("{0}{1}{2,39}",
				Environment.NewLine,
				"Weapon Abilities:",
				"Spell Abilities:");

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			i = Math.Min((long)weaponValues[0], (long)spellValues[0]);

			j = Math.Max((long)weaponValues[weaponValues.Count - 1], (long)spellValues[spellValues.Count - 1]);

			while (i <= j)
			{
				Globals.Out.WriteLine();

				if (Enum.IsDefined(typeof(Enums.Weapon), i))
				{
					weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

					Debug.Assert(weapon != null);

					Globals.Out.Write(" {0,-5}: {1,3}%",
						weapon.Name,
						GetWeaponAbilities(i));
				}
				else
				{
					Globals.Out.Write("{0,12}", "");
				}

				if (Enum.IsDefined(typeof(Enums.Spell), i))
				{
					spell = Globals.Engine.GetSpells((Enums.Spell)i);

					Debug.Assert(spell != null);

					Globals.Out.Write("{0,29}{1,-5}: {2,3}% / {3}%",
						"",
						spell.Name,
						args.GetSpellAbilities(i),
						GetSpellAbilities(i));
				}

				i++;
			}

			Globals.Out.WriteLine("{0}{0}{1}{2,-30}{3}{4,-5}",
				Environment.NewLine,
				"Gold: ",
				HeldGold,
				"In bank: ",
				BankGold);

			Globals.Out.WriteLine("{0}Armor: {1}  Armor Expertise: {2}%",
				Environment.NewLine,
				args.ArmorString.PadTRight(31, ' '),
				ArmorExpertise);

			var wcg = Globals.Engine.GetWeightCarryableGronds(args.Monster.Hardiness);

			Globals.Out.WriteLine("{0}Weight carried: {1}/{2} Gronds (One Grond = Ten DOS)",
				Environment.NewLine,
				args.Weight,
				wcg);

		Cleanup:

			return rc;
		}

		public virtual RetCode CopyProperties(ICharacter character)
		{
			RetCode rc;

			if (character == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			Uid = character.Uid;

			IsUidRecycled = character.IsUidRecycled;

			Name = Globals.CloneInstance(character.Name);

			Debug.Assert(Synonyms == null && character.Synonyms == null);

			Seen = character.Seen;

			ArticleType = character.ArticleType;

			Gender = character.Gender;

			Status = character.Status;

			Debug.Assert(Stats.Length == character.Stats.Length);

			for (var i = 0; i < Stats.Length; i++)
			{
				SetStats(i, character.GetStats(i));
			}

			Debug.Assert(SpellAbilities.Length == character.SpellAbilities.Length);

			for (var i = 0; i < SpellAbilities.Length; i++)
			{
				SetSpellAbilities(i, character.GetSpellAbilities(i));
			}

			Debug.Assert(WeaponAbilities.Length == character.WeaponAbilities.Length);

			for (var i = 0; i < WeaponAbilities.Length; i++)
			{
				SetWeaponAbilities(i, character.GetWeaponAbilities(i));
			}

			ArmorExpertise = character.ArmorExpertise;

			HeldGold = character.HeldGold;

			BankGold = character.BankGold;

			ArmorClass = character.ArmorClass;

			Debug.Assert(Weapons.Length == character.Weapons.Length);

			for (var i = 0; i < Weapons.Length; i++)
			{
				GetWeapons(i).Name = Globals.CloneInstance(character.GetWeapons(i).Name);

				GetWeapons(i).IsPlural = character.GetWeapons(i).IsPlural;

				GetWeapons(i).PluralType = character.GetWeapons(i).PluralType;

				GetWeapons(i).ArticleType = character.GetWeapons(i).ArticleType;

				GetWeapons(i).Complexity = character.GetWeapons(i).Complexity;

				GetWeapons(i).Type = character.GetWeapons(i).Type;

				GetWeapons(i).Dice = character.GetWeapons(i).Dice;

				GetWeapons(i).Sides = character.GetWeapons(i).Sides;
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Character

		public Character()
		{
			SetUidIfInvalid = SetCharacterUidIfInvalid;

			IsUidRecycled = true;

			Name = "";

			Stats = new long[(long)EnumUtil.GetLastValue<Enums.Stat>() + 1];

			SpellAbilities = new long[(long)EnumUtil.GetLastValue<Enums.Spell>() + 1];

			WeaponAbilities = new long[(long)EnumUtil.GetLastValue<Enums.Weapon>() + 1];

			Weapons = new Classes.ICharacterWeapon[]
			{
				Globals.CreateInstance<Classes.ICharacterWeapon>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.ICharacterWeapon>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.ICharacterWeapon>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.ICharacterWeapon>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
