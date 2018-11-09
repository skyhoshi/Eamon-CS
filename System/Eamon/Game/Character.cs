
// Character.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Character : GameBase, ICharacter
	{
		#region Protected Fields

		protected long _heldGold;

		protected long _bankGold;

		#endregion

		#region Public Properties

		#region Interface ICharacter

		public virtual Enums.Gender Gender { get; set; }

		public virtual Enums.Status Status { get; set; }

		public virtual long[] Stats { get; set; }

		public virtual long[] SpellAbilities { get; set; }

		public virtual long[] WeaponAbilities { get; set; }

		public virtual long ArmorExpertise { get; set; }

		public virtual long HeldGold
		{
			get
			{
				return _heldGold;
			}

			set
			{
				_heldGold = value;

				if (_heldGold < Constants.MinGoldValue)
				{
					_heldGold = Constants.MinGoldValue;
				}
				else if (_heldGold > Constants.MaxGoldValue)
				{
					_heldGold = Constants.MaxGoldValue;
				}
			}
		}

		public virtual long BankGold
		{
			get
			{
				return _bankGold;
			}

			set
			{
				_bankGold = value;

				if (_bankGold < Constants.MinGoldValue)
				{
					_bankGold = Constants.MinGoldValue;
				}
				else if (_bankGold > Constants.MaxGoldValue)
				{
					_bankGold = Constants.MaxGoldValue;
				}
			}
		}

		public virtual Enums.Armor ArmorClass { get; set; }

		public virtual Classes.ICharacterArtifact Armor { get; set; }

		public virtual Classes.ICharacterArtifact Shield { get; set; }

		public virtual Classes.ICharacterArtifact[] Weapons { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
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

		#region Interface IGameBase

		public override void SetParentReferences()
		{
			Armor.Parent = this;

			Shield.Parent = this;

			foreach (var w in Weapons)
			{
				w.Parent = this;
			}
		}

		public override string GetPluralName(string fieldName, StringBuilder buf)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			buf.Append(Name);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override string GetDecoratedName(string fieldName, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			buf.Append(Name);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
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

			buf.AppendPrint("You are the {0} {1}.",
				EvalGender("Mighty", "Fair", "Androgynous"),
				name);

		Cleanup:

			return rc;
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

		public virtual Classes.ICharacterArtifact GetWeapons(long index)
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

		public virtual void SetWeapons(long index, Classes.ICharacterArtifact value)
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

		public virtual bool IsArmorActive()
		{
			return Armor.IsActive();
		}

		public virtual bool IsShieldActive()
		{
			return Shield.IsActive();
		}

		public virtual bool IsWeaponActive(long index)
		{
			Debug.Assert(index >= 0 && index < Weapons.Length);

			return GetWeapons(index).IsActive();
		}

		public virtual T EvalGender<T>(T maleValue, T femaleValue, T neutralValue)
		{
			return Globals.Engine.EvalGender(Gender, maleValue, femaleValue, neutralValue);
		}

		public virtual RetCode GetBaseOddsToHit(Classes.ICharacterArtifact weapon, ref long baseOddsToHit)
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

			a = GetWeaponAbilities((Enums.Weapon)weapon.Field2);

			d = weapon.Field1;

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
					weapon = Globals.Engine.GetWeapons((Enums.Weapon)GetWeapons(i).Field2);

					Debug.Assert(weapon != null);

					buf.AppendFormat("{0}{1,2}. {2} ({3,-8}/{4,3}%/{5,2}D{6,-2}/{7}H)",
						Environment.NewLine,
						i + 1,
						capitalize ? Globals.Engine.Capitalize(GetWeapons(i).Name.PadTRight(31, ' ')) : GetWeapons(i).Name.PadTRight(31, ' '),
						weapon.Name,
						GetWeapons(i).Field1,
						GetWeapons(i).Field3,
						GetWeapons(i).Field4,
						GetWeapons(i).Field5);
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

			Globals.Out.Print("{0,-36}Gender: {1,-9}Damage Taken: {2}/{3}",
				args.Monster.Name.ToUpper(),
				EvalGender("Male", "Female", "Neutral"),
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

			Globals.Out.Print("Armor: {0}  Armor Expertise: {1}%",
				args.ArmorString.PadTRight(31, ' '),
				ArmorExpertise);

			var wcg = Globals.Engine.GetWeightCarryableGronds(args.Monster.Hardiness);

			Globals.Out.Print("Weight carried: {0}/{1} Gronds (One Grond = Ten DOS)",
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

			Globals.Engine.CopyCharacterArtifactFields(Armor, character.Armor);

			Globals.Engine.CopyCharacterArtifactFields(Shield, character.Shield);

			Debug.Assert(Weapons.Length == character.Weapons.Length);

			for (var i = 0; i < Weapons.Length; i++)
			{
				Globals.Engine.CopyCharacterArtifactFields(GetWeapons(i), character.GetWeapons(i));
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Character

		public Character()
		{
			Stats = new long[(long)EnumUtil.GetLastValue<Enums.Stat>() + 1];

			SpellAbilities = new long[(long)EnumUtil.GetLastValue<Enums.Spell>() + 1];

			WeaponAbilities = new long[(long)EnumUtil.GetLastValue<Enums.Weapon>() + 1];

			Armor = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
			{
				x.Parent = this;
			});

			Shield = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
			{
				x.Parent = this;
			});

			Weapons = new Classes.ICharacterArtifact[]
			{
				Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
