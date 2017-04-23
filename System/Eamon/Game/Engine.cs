
// Engine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Engine : IEngine
	{
		#region Protected Properties

		protected virtual Random Rand { get; set; }

		protected virtual string[] NumberStrings { get; set; }

		protected virtual string[] FieldDescNames { get; set; }

		protected virtual string[] StatusNames { get; set; }

		protected virtual string[] ClothingNames { get; set; }

		protected virtual string[] CombatCodeDescs { get; set; }

		protected virtual string[] LightLevelNames { get; set; }

		protected virtual Classes.IStat[] Stats { get; set; }

		protected virtual Classes.ISpell[] Spells { get; set; }

		protected virtual Classes.IWeapon[] Weapons { get; set; }

		protected virtual Classes.IArmor[] Armors { get; set; }

		protected virtual Classes.IGender[] Genders { get; set; }

		protected virtual Classes.IDirection[] Directions { get; set; }

		protected virtual Classes.IFriendliness[] Friendlinesses { get; set; }

		protected virtual Classes.IRoomType[] RoomTypes { get; set; }

		protected virtual Classes.IArtifactType[] ArtifactTypes { get; set; }

		protected virtual IDictionary<Tuple<Enums.Weapon, long>, string> AttackDescs { get; set; }

		protected virtual IDictionary<Tuple<Enums.Weapon, long>, string> MissDescs { get; set; }

		#endregion

		#region Public Properties

		public virtual IDictionary<long, Func<string>> MacroFuncs { get; set; }

		public virtual string[] Preps { get; set; }

		public virtual string[] Articles { get; set; }

		public virtual long NumCacheItems { get; set; }

		public virtual string ProvidingLightDesc { get; set; }

		public virtual string ReadyWeaponDesc { get; set; }

		public virtual string BrokenDesc { get; set; }

		public virtual string EmptyDesc { get; set; }

		public virtual string BlastDesc { get; set; }

		public virtual string UnknownName { get; set; }

		#endregion

		#region Public Methods

		#region Interface IEngine

		public virtual string GetPreps(long index)
		{
			return Preps[index];
		}

		public virtual string GetArticles(long index)
		{
			return Articles[index];
		}

		public virtual string GetNumberStrings(long index)
		{
			return NumberStrings[index];
		}

		public virtual string GetFieldDescNames(long index)
		{
			return FieldDescNames[index];
		}

		public virtual string GetFieldDescNames(Enums.FieldDesc fieldDesc)
		{
			return Enum.IsDefined(typeof(Enums.FieldDesc), fieldDesc) ? GetFieldDescNames((long)fieldDesc) : UnknownName;
		}

		public virtual string GetStatusNames(long index)
		{
			return StatusNames[index];
		}

		public virtual string GetStatusNames(Enums.Status status)
		{
			return Enum.IsDefined(typeof(Enums.Status), status) ? GetStatusNames((long)status) : UnknownName;
		}

		public virtual string GetClothingNames(long index)
		{
			return ClothingNames[index];
		}

		public virtual string GetClothingNames(Enums.Clothing clothing)
		{
			return Enum.IsDefined(typeof(Enums.Clothing), clothing) ? GetClothingNames((long)clothing) : UnknownName;
		}

		public virtual string GetCombatCodeDescs(long index)
		{
			return CombatCodeDescs[index];
		}

		public virtual string GetCombatCodeDescs(Enums.CombatCode combatCode)
		{
			return Enum.IsDefined(typeof(Enums.CombatCode), combatCode) ? GetCombatCodeDescs((long)combatCode + 2) : UnknownName;
		}

		public virtual string GetLightLevelNames(long index)
		{
			return LightLevelNames[index];
		}

		public virtual string GetLightLevelNames(Enums.LightLevel lightLevel)
		{
			return Enum.IsDefined(typeof(Enums.LightLevel), lightLevel) ? GetLightLevelNames((long)lightLevel) : UnknownName;
		}

		public virtual Classes.IStat GetStats(long index)
		{
			return Stats[index];
		}

		public virtual Classes.IStat GetStats(Enums.Stat stat)
		{
			return Enum.IsDefined(typeof(Enums.Stat), stat) ? GetStats((long)stat - 1) : null;
		}

		public virtual Classes.ISpell GetSpells(long index)
		{
			return Spells[index];
		}

		public virtual Classes.ISpell GetSpells(Enums.Spell spell)
		{
			return Enum.IsDefined(typeof(Enums.Spell), spell) ? GetSpells((long)spell - 1) : null;
		}

		public virtual Classes.IWeapon GetWeapons(long index)
		{
			return Weapons[index];
		}

		public virtual Classes.IWeapon GetWeapons(Enums.Weapon weapon)
		{
			return Enum.IsDefined(typeof(Enums.Weapon), weapon) ? GetWeapons((long)weapon - 1) : null;
		}

		public virtual Classes.IArmor GetArmors(long index)
		{
			return Armors[index];
		}

		public virtual Classes.IArmor GetArmors(Enums.Armor armor)
		{
			return Enum.IsDefined(typeof(Enums.Armor), armor) ? GetArmors((long)armor) : null;
		}

		public virtual Classes.IGender GetGenders(long index)
		{
			return Genders[index];
		}

		public virtual Classes.IGender GetGenders(Enums.Gender gender)
		{
			return Enum.IsDefined(typeof(Enums.Gender), gender) ? GetGenders((long)gender) : null;
		}

		public virtual Classes.IDirection GetDirections(long index)
		{
			return Directions[index];
		}

		public virtual Classes.IDirection GetDirections(Enums.Direction direction)
		{
			return Enum.IsDefined(typeof(Enums.Direction), direction) ? GetDirections((long)direction - 1) : null;
		}

		public virtual Classes.IFriendliness GetFriendlinesses(long index)
		{
			return Friendlinesses[index];
		}

		public virtual Classes.IFriendliness GetFriendlinesses(Enums.Friendliness friendliness)
		{
			return Enum.IsDefined(typeof(Enums.Friendliness), friendliness) ? GetFriendlinesses((long)friendliness - 1) : null;
		}

		public virtual Classes.IRoomType GetRoomTypes(long index)
		{
			return RoomTypes[index];
		}

		public virtual Classes.IRoomType GetRoomTypes(Enums.RoomType roomType)
		{
			return Enum.IsDefined(typeof(Enums.RoomType), roomType) ? GetRoomTypes((long)roomType) : null;
		}

		public virtual Classes.IArtifactType GetArtifactTypes(long index)
		{
			return ArtifactTypes[index];
		}

		public virtual Classes.IArtifactType GetArtifactTypes(Enums.ArtifactType artifactType)
		{
			return IsValidArtifactType(artifactType) ? GetArtifactTypes((long)artifactType) : null;
		}

		public virtual bool IsSuccess(RetCode rc)
		{
			return (long)rc >= (long)RetCode.Success;
		}

		public virtual bool IsFailure(RetCode rc)
		{
			return !IsSuccess(rc);
		}

		public virtual bool IsValidPluralType(Enums.PluralType pluralType)
		{
			return Enum.IsDefined(typeof(Enums.PluralType), pluralType) || (long)pluralType > 1000;
		}

		public virtual bool IsValidArtifactType(Enums.ArtifactType artifactType)
		{
			return Enum.IsDefined(typeof(Enums.ArtifactType), artifactType) && artifactType != Enums.ArtifactType.None;
		}

		public virtual bool IsValidArtifactArmor(long armor)
		{
			return Enum.IsDefined(typeof(Enums.Armor), armor) && (armor == (long)Enums.Armor.ClothesShield || armor % 2 == 0);
		}

		public virtual bool IsValidMonsterArmor(long armor)
		{
			return armor >= 0;
		}

		public virtual bool IsValidMonsterCourage(long courage)
		{
			return courage >= 0 && courage <= 200;
		}

		public virtual bool IsValidMonsterFriendliness(Enums.Friendliness friendliness)
		{
			return Enum.IsDefined(typeof(Enums.Friendliness), friendliness) || IsValidMonsterFriendlinessPct(friendliness);
		}

		public virtual bool IsValidMonsterFriendlinessPct(Enums.Friendliness friendliness)
		{
			return (long)friendliness >= 100 && (long)friendliness <= 200;
		}

		public virtual bool IsValidDirection(Enums.Direction dir)
		{
			var module = GetModule();

			var numDirs = module != null ? module.NumDirs : 6;

			return (long)dir <= numDirs;
		}

		public virtual bool IsValidRoomUid01(long roomUid)
		{
			return roomUid != 0 && roomUid < 1001;
		}

		public virtual bool IsValidRoomDirectionDoorUid01(long roomUid)
		{
			return roomUid > 1000;
		}

		public virtual bool IsArtifactFieldStrength(long value)
		{
			return value >= 1000;
		}

		public virtual bool IsUnmovable(long weight)
		{
			return weight == -999 || weight == 999;
		}

		public virtual bool IsUnmovable01(long weight)
		{
			return weight == -999;
		}

		public virtual long GetWeightCarryableGronds(long hardiness)
		{
			return hardiness * 10;
		}

		public virtual long GetWeightCarryableDos(long hardiness)
		{
			return GetWeightCarryableGronds(hardiness) * 10;
		}

		public virtual long GetIntellectBonusPct(long intellect)
		{
			return (intellect - 13) * 2;
		}

		public virtual long GetCharmMonsterPct(long charisma)
		{
			return (charisma - 10) * 2;
		}

		public virtual long GetPluralTypeEffectUid(Enums.PluralType pluralType)
		{
			return (long)pluralType > 1000 ? (long)pluralType - 1000 : 0;
		}

		public virtual long GetArmorFactor(long armorUid, long shieldUid)		// Note: test thoroughly!
		{
			long af = 0;

			if (armorUid > 0)
			{
				var artifact = Globals.ADB[armorUid];

				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactClass(Enums.ArtifactType.Wearable);

				Debug.Assert(ac != null);

				var f = ac.Field5 / 2;

				if (f > 3)
				{
					f = 3;
				}
				
				af -= (10 * f);

				if (f == 3)
				{
					af -= 30;
				}
			}

			if (shieldUid > 0)
			{
				var artifact = Globals.ADB[shieldUid];

				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactClass(Enums.ArtifactType.Wearable);

				Debug.Assert(ac != null);

				af -= (5 * ac.Field5);
			}

			return af;
		}

		public virtual long GetCharismaFactor(long charisma)
		{
			var f = GetCharmMonsterPct(charisma);

			if (f > 28)
			{
				f = 28;
			}

			return f;
		}

		public virtual long GetMonsterFriendlinessPct(Enums.Friendliness friendliness)
		{
			return (long)friendliness - 100;
		}

		public virtual long GetArtifactFieldStrength(long value)
		{
			return value - 1000;
		}

		public virtual long GetMerchantAskPrice(double price, double rtio)
		{
			return (long)((price) * (rtio) + .5);
		}

		public virtual long GetMerchantBidPrice(double price, double rtio)
		{
			return (long)((price) / (rtio) + .5);
      }

		public virtual long GetMerchantAdjustedCharisma(long charisma)
		{
			var j = RollDice01(1, 11, -6);

			var c2 = charisma + j;

			var stat = GetStats(Enums.Stat.Charisma);

			Debug.Assert(stat != null);

			if (c2 < stat.MinValue)
			{
				c2 = stat.MinValue;
			}
			else if (c2 > stat.MaxValue)
			{
				c2 = stat.MaxValue;
			}

			return c2;
		}

		public virtual double GetMerchantRtio(long charisma)
		{
			var stat = GetStats(Enums.Stat.Charisma);

			Debug.Assert(stat != null);

			var min = 0;

			var max = 1;

			var a = 0.70;

			var b = 1.30;

			var x = (double)((stat.MaxValue - stat.MinValue) - (charisma - stat.MinValue)) / (double)(stat.MaxValue - stat.MinValue);

			return (((b - a) * (x - min)) / (max - min)) + a;
		}

		public virtual bool IsCharYOrN(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'Y' || ch == 'N';
		}

		public virtual bool IsCharSOrTOrROrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'S' || ch == 'T' || ch == 'R' || ch == 'X';
		}

		public virtual bool IsChar0Or1(char ch)
		{
			return ch == '0' || ch == '1';
		}

		public virtual bool IsChar0To2(char ch)
		{
			return ch >= '0' && ch <= '2';
		}

		public virtual bool IsChar0To3(char ch)
		{
			return ch >= '0' && ch <= '3';
		}

		public virtual bool IsChar1To3(char ch)
		{
			return ch >= '1' && ch <= '3';
		}

		public virtual bool IsCharDigit(char ch)
		{
			return Char.IsDigit(ch);
		}

		public virtual bool IsCharDigitOrX(char ch)
		{
			return Char.IsDigit(ch) || Char.ToUpper(ch) == 'X';
		}

		public virtual bool IsCharPlusMinusDigit(char ch)
		{
			return ch == '+' || ch == '-' || Char.IsDigit(ch);
		}

		public virtual bool IsCharAlpha(char ch)
		{
			return Char.IsLetter(ch);
		}

		public virtual bool IsCharAlphaSpace(char ch)
		{
			return Char.IsLetter(ch) || Char.IsWhiteSpace(ch);
		}

		public virtual bool IsCharAlnum(char ch)
		{
			return Char.IsLetterOrDigit(ch);
		}

		public virtual bool IsCharAlnumSpace(char ch)
		{
			return Char.IsLetterOrDigit(ch) || Char.IsWhiteSpace(ch);
		}

		public virtual bool IsCharPrint(char ch)
		{
			return Char.IsControl(ch) == false;
		}

		public virtual bool IsCharPound(char ch)
		{
			return ch == '#';
		}

		public virtual bool IsCharQuote(char ch)
		{
			return ch == '\'' || ch == '`' || ch == '"';
		}

		public virtual bool IsCharAny(char ch)
		{
			return true;
		}

		public virtual bool IsCharAnyButDquoteCommaColon(char ch)
		{
			return ch != '"' && ch != ',' && ch != ':';
		}

		public virtual char ModifyCharToUpper(char ch)
		{
			return Char.ToUpper(ch);
		}

		public virtual char ModifyCharToNullOrX(char ch)
		{
			return Char.ToUpper(ch) == 'X' ? 'X' : '\0';
		}

		public virtual char ModifyCharToNull(char ch)
		{
			return '\0';
		}

		public virtual Enums.Direction GetDirection(string printedName)
		{
			Enums.Direction result = 0;

			Debug.Assert(!string.IsNullOrWhiteSpace(printedName));

			var module = GetModule();

			var numDirs = module != null ? module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Enums.Direction>();

			for (var i = 0; i < numDirs; i++)
			{
				if (string.Equals(GetDirections(i).PrintedName, printedName, StringComparison.OrdinalIgnoreCase))
				{
					result = directionValues[i];

					break;
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirections(i).PrintedName.StartsWith(printedName, StringComparison.OrdinalIgnoreCase) || GetDirections(i).PrintedName.EndsWith(printedName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			return result;
		}

		public virtual IConfig GetConfig()
		{
			return Globals?.Database?.ConfigTable.Records.FirstOrDefault();
		}

		public virtual IGameState GetGameState()
		{
			return Globals?.Database?.GameStateTable.Records.FirstOrDefault();
		}

		public virtual IModule GetModule()
		{
			return Globals?.Database?.ModuleTable.Records.FirstOrDefault();
		}

		public virtual string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal)
		{
			StringBuilder buf;
			int i, p, q, sz;
			string result;

			if (bufSize < 8 || number < 0)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			buf = new StringBuilder(Constants.BufSize);

			buf.Append(fillChar, (int)bufSize);

			p = 0;

			q = (int)bufSize;

			if (number > 0)
			{
				buf.Replace(p, 4, string.Format("{0,2}. ", number));

				p += 4;
			}

			if (msg != null)
			{
				sz = msg.Length;

				for (i = 0; i < sz && p < q; i++)
				{
					buf[p++] = msg[i];
				}
			}

			if (emptyVal != null)
			{
				sz = emptyVal.Length;

				p = Math.Max(q - (sz + 4), 0);

				buf.Replace(p, Math.Min(sz + 4, q), string.Format("[{0}]{1} ", emptyVal, fillChar == ' ' ? ':' : fillChar));
			}
			else
			{
				p = q - 2;

				buf.Replace(p, 2, string.Format("{0} ", fillChar == ' ' ? ':' : fillChar));
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg)
		{
			StringBuilder buf;
			int p, q, sz;
			string result;
			string s;

			if (bufSize < 8 || offset < 0 || offset > bufSize - 1)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			buf = new StringBuilder(Constants.BufSize);

			buf.Append(fillChar, (int)bufSize);

			p = 0;

			q = (int)bufSize;

			s = stringVal ?? longVal.ToString();

			sz = Math.Min(s.Length, q);

			buf.Replace(p, sz, s);

			p += sz;

			if (lookupMsg != null)
			{
				p = (int)offset;

				s = string.Format("[{0}]", lookupMsg.Length > (q - p) - 2 ? lookupMsg.Substring(0, (q - p) - 2) : lookupMsg);

				sz = Math.Min(s.Length, q - p);

				buf.Replace(p, sz, s);

				p += sz;
			}

			buf.Length = p;

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true)
		{
			int i, p, q, r;
			int currMargin;
			bool hyphenSeen;
			string result;
			string line;

			if (str == null || buf == null || margin < 1)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (clearBuf)
			{
				buf.Clear();
			}

			if (args != null)
			{
				if (args.CurrColumn == margin)
				{
					buf.Append(Environment.NewLine);

					args.CurrColumn = 0;
				}

				currMargin = (int)(margin - args.CurrColumn);
			}
			else
			{
				currMargin = (int)margin;
			}

			var delims = Environment.NewLine.Length > 1 ?
					new string[] { Environment.NewLine, Environment.NewLine[1].ToString() } :
					new string[] { Environment.NewLine };

			var lines = str.Split(delims, StringSplitOptions.None);

			for (i = 0; i < lines.Length; i++)
			{
				if (i > 0)
				{
					buf.Append(Environment.NewLine);
				}

				line = lines[i];

				p = 0;

				q = line.Length;

				while (true)
				{
					if (p + currMargin >= q)
					{
						buf.Append(line.Substring(p));

						if (args != null)
						{
							args.CurrColumn = (q - p);
						}

						p += (q - p);

						break;
					}
					else
					{
						r = p + currMargin;

						hyphenSeen = false;

						while (r > p && !Char.IsWhiteSpace(line[r]) && line[r] != '-')
						{
							r--;
						}

						if (r > p)
						{
							if (line[r] == '-')
							{
								hyphenSeen = true;
							}

							buf.Append(line.Substring(p, (r - p)));

							p += (r - p) + 1;

							if (p < q && Char.IsWhiteSpace(line[p]) && (p + 2 < q) && (!Char.IsWhiteSpace(line[p + 1]) || !Char.IsWhiteSpace(line[p + 2])))
							{
								p++;
							}

							if (p < q && Char.IsWhiteSpace(line[p]) && (p + 1 < q) && !Char.IsWhiteSpace(line[p + 1]))
							{
								p++;
							}

							if (hyphenSeen)
							{
								buf.Append('-');
							}
						}
						else
						{
							if (r > 0 || args == null || (!Char.IsWhiteSpace(args.LastChar) && args.LastChar != '-'))
							{
								buf.Append(line.Substring(p, currMargin));

								p += currMargin;
							}
						}

						buf.Append(Environment.NewLine);
					}

					currMargin = (int)margin;
				}
			}

			if (args != null)
			{
				args.LastChar = buf.Length > 0 ? buf[buf.Length - 1] : '\0';
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string WordWrap(string str, StringBuilder buf, bool clearBuf = true)
		{
			var config = GetConfig();

			return WordWrap(str, buf, config != null ? config.WordWrapMargin : Constants.RightMargin, null, clearBuf);
		}

		public virtual string GetNumberString(long num, bool addSpace, StringBuilder buf)
		{
			string result;

			if (buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			buf.SetFormat("{0}{1}", 
				num >= 0 && num <= 10 ? GetNumberStrings(num) : num.ToString(), 
				addSpace ? " " : "");

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetAttackDescString(Enums.Weapon weapon, long roll)
		{
			string result = null;

			AttackDescs.TryGetValue(new Tuple<Enums.Weapon, long>(weapon, roll), out result);

			return result ?? "attack{0}";
		}

		public virtual string GetMissDescString(Enums.Weapon weapon, long roll)
		{
			string result = null;

			MissDescs.TryGetValue(new Tuple<Enums.Weapon, long>(weapon, roll), out result);

			return result ?? "Missed";
		}

		public virtual RetCode RollDice(long numDice, long numSides, ref long[] dieRolls)
		{
			RetCode rc;

			if (numDice < 0 || numSides < 0 || dieRolls == null || dieRolls.Length < numDice)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (var i = 0; i < numDice; i++)
			{
				dieRolls[i] = numSides > 0 ? Rand.Next((int)numSides) + 1 : 0;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode RollDice(long numDice, long numSides, long modifier, ref long result)
		{
			RetCode rc;

			long[] dieRolls = new long[numDice > 0 ? numDice : 1];

			rc = RollDice(numDice, numSides, ref dieRolls);

			if (IsSuccess(rc))
			{
				result = dieRolls.Sum() + modifier;
			}

			return rc;
		}

		public virtual long RollDice01(long numDice, long numSides, long modifier)
		{
			long result = 0;

			var rc = RollDice(numDice, numSides, modifier, ref result);

			Debug.Assert(IsSuccess(rc));

			return result;
		}

		public virtual RetCode SumHighestRolls(long[] dieRolls, long numDieRolls, long numRollsToSum, ref long result)
		{
			RetCode rc;

			if (dieRolls == null || numDieRolls < 1 || numRollsToSum < 0 || numRollsToSum > numDieRolls)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			Array.Sort(dieRolls);

			result = dieRolls.Skip((int)(numDieRolls - numRollsToSum)).Take((int)numRollsToSum).Sum();

		Cleanup:

			return rc;
		}

		public virtual RetCode BuildCommandList(IList<ICommand> commands, Enums.CommandType cmdType, StringBuilder buf, ref bool newSeen)
		{
			StringBuilder buf02, buf03;
			RetCode rc;
			int i;

			if (commands == null || !Enum.IsDefined(typeof(Enums.CommandType), cmdType) || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf02 = new StringBuilder(Constants.BufSize);

			buf03 = new StringBuilder(Constants.BufSize);

			i = 0;

			foreach (var c in commands)
			{
				if (cmdType == Enums.CommandType.None || c.Type == cmdType)
				{
					i++;

					buf02.SetFormat("{0}{1}",
						c.GetPrintedVerb(),
						c.IsNew ? " (*)" : "");

					buf03.AppendFormat("{0,-15}{1}",
						buf02.ToString(),
						(i % 5) == 0 ? Environment.NewLine : "");

					if (c.IsNew)
					{
						newSeen = true;
					}
				}
			}

			buf.AppendFormat("{0}{1}{2}",
				buf03.Length > 0 ? Environment.NewLine : "",
				buf03.Length > 0 ? buf03.ToString() : "(None)",
				i == 0 || (i % 5) != 0 ? Environment.NewLine : "");

		Cleanup:

			return rc;
		}

		public virtual string Capitalize(string str)
		{
			StringBuilder buf;
			bool spaceSeen;
			int p, q, sz;
			string result;

			if (str == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			spaceSeen = true;

			buf = new StringBuilder(Constants.BufSize);

			buf.Append(str);

			sz = buf.Length;

			for (p = 0, q = sz; p < q; p++)
			{
				if (spaceSeen)
				{
					if (Char.IsLetter(buf[p]))
					{
						buf[p] = Char.ToUpper(buf[p]);

						spaceSeen = false;
					}
				}
				else
				{
					if (Char.IsWhiteSpace(buf[p]))
					{
						spaceSeen = true;
					}
				}
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual void UnlinkOnFailure()
		{
			try
			{
				Globals.File.Delete("EAMONCFG.XML");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			try
			{
				Globals.File.Delete("FRESHMEAT.XML");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			try
			{
				Globals.File.Delete("SAVEGAME.XML");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}
		}

		public virtual void TruncatePluralTypeEffectDesc(Enums.PluralType pluralType, long maxSize)
		{
			if (maxSize < 0)
			{
				// PrintError

				goto Cleanup;
			}

			var effectUid = GetPluralTypeEffectUid(pluralType);

			if (effectUid > 0)
			{
				var effect = Globals.EDB[effectUid];

				if (effect != null && effect.Desc.Length > maxSize)
				{
					effect.Desc = effect.Desc.Substring(0, (int)maxSize);
				}
			}

		Cleanup:

			;
		}

		public virtual void TruncatePluralTypeEffectDesc(IEffect effect)
		{
			if (effect == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (effect.Desc.Length > Constants.ArtNameLen && Globals.Database.ArtifactTable.Records.FirstOrDefault(a => GetPluralTypeEffectUid(a.PluralType) == effect.Uid) != null)
			{
				effect.Desc = effect.Desc.Substring(0, Constants.ArtNameLen);
			}

			if (effect.Desc.Length > Constants.MonNameLen && Globals.Database.MonsterTable.Records.FirstOrDefault(m => GetPluralTypeEffectUid(m.PluralType) == effect.Uid) != null)
			{
				effect.Desc = effect.Desc.Substring(0, Constants.MonNameLen);
			}

		Cleanup:

			;
		}

		public virtual void GetPossessiveName(StringBuilder buf)
		{
			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (buf.EndsWith("s", true))
			{
				buf.Append("'");
			}
			else
			{
				buf.Append("'s");
			}

		Cleanup:

			;
		}

		public virtual long FindIndex<T>(T[] array, long startIndex, long count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}

			if (count < 0 || startIndex > array.Length - count)
			{
				throw new ArgumentOutOfRangeException("count");
			}

			if (match == null)
			{
				throw new ArgumentNullException("match");
			}

			long endIndex = startIndex + count;

			for (long i = startIndex; i < endIndex; i++)
			{
				if (match(array[i]))
				{
					return i;
				}
			}

			return -1;
		}

		public virtual long FindIndex<T>(T[] array, long startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			return FindIndex(array, startIndex, array.Length - startIndex, match);
		}

		public virtual long FindIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			return FindIndex(array, 0, array.Length, match);
		}

		public virtual RetCode SplitPath(string fullPath, ref string directory, ref string fileName, ref string extension, bool appendDirectorySeparatorChar = true)
		{
			RetCode rc;

			if (fullPath == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			directory = Globals.Path.GetDirectoryName(fullPath);

			fileName = Globals.Path.GetFileNameWithoutExtension(fullPath);

			extension = Globals.Path.GetExtension(fullPath);

			var directorySeparatorString = Globals.Path.DirectorySeparatorChar.ToString();

			if (appendDirectorySeparatorChar && directory.Length > 0 && !directory.EndsWith(directorySeparatorString))
			{
				directory += directorySeparatorString;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode StripPrepsAndArticles(StringBuilder buf, ref bool mySeen)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			mySeen = false;

			foreach (var p in Preps)
			{
				var s = p + " ";

				if (buf.StartsWith(s, true))
				{
					buf.Remove(0, s.Length);

					buf.TrimStart();

					break;
				}
			}

			foreach (var a in Articles)
			{
				var s = a + " ";

				if (buf.StartsWith(s, true))
				{
					buf.Remove(0, s.Length);

					buf.TrimStart();

					if (a == "my" || a == "your")
					{
						mySeen = true;
					}

					break;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual void PrintTitle(string title, bool inBox)
		{
			long spaces;
			long size;

			if (string.IsNullOrWhiteSpace(title))
			{
				// PrintError

				goto Cleanup;
			}

			size = title.Length;

			if (inBox)
			{
				Globals.Out.Write("{0}{1}|",
					Globals.LineSep,
					Environment.NewLine);
			}

			spaces = ((Constants.RightMargin - 2) / 2) - (size / 2);

			Globals.Out.Write("{0}{1}", new string(' ', (int)spaces), title);

			if (inBox)
			{
				Globals.Out.Write("{0}|{1}{2}",
					new string(' ', (int)((Constants.RightMargin - 1) - (1 + spaces + size))),
					Environment.NewLine,
					Globals.LineSep);
			}

			Globals.Out.WriteLine();

		Cleanup:

			;
		}

		public virtual void PrintEffectDesc(IEffect effect, bool printFinalNewLine = true)
		{
			Debug.Assert(effect != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, effect.Desc, printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintEffectDesc(long effectUid, bool printFinalNewLine = true)
		{
			var effect = Globals.EDB[effectUid];

			PrintEffectDesc(effect, printFinalNewLine);
		}

		public virtual RetCode GetRecordNameList(IList<IHaveListedName> records, Enums.ArticleType articleType, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			StringBuilder buf01;
			RetCode rc;
			long i;

			if (records == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf01 = new StringBuilder(Constants.BufSize);

			for (i = 0; i < records.Count; i++)
			{
				var x = records[(int)i];

				buf.AppendFormat("{0}{1}",
					i == 0 ? "" : i == records.Count - 1 ? " and " : ", ",
					x.GetDecoratedName
					(
						x.GetNameField(),
						articleType == Enums.ArticleType.None || articleType == Enums.ArticleType.The ? articleType : x.ArticleType,
						false,
						showCharOwned,
						showStateDesc,
						groupCountOne,
						buf01
					)
				);
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode GetRecordNameCount(IList<IHaveListedName> records, string name, bool exactMatch, ref long count)
		{
			RetCode rc;

			if (records == null || string.IsNullOrWhiteSpace(name))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			count = 0;

			foreach (var x in records)
			{
				if (exactMatch)
				{
					if (string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
					{
						count++;
					}
				}
				else
				{
					if (x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
					{
						count++;
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode ListRecords(IList<IHaveListedName> records, bool capitalize, bool showExtraInfo, StringBuilder buf)
		{
			RetCode rc;
			long i;

			if (records == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

			for (i = 0; i < records.Count; i++)
			{
				var x = records[(int)i];

				var a = x as IArtifact;

				if (showExtraInfo && a != null && a.IsWeapon01())
				{
					var ac = a.GetArtifactClass(artClasses);

					Debug.Assert(ac != null);

					var weapon = GetWeapons((Enums.Weapon)ac.Field6);

					Debug.Assert(weapon != null);

					buf.AppendFormat("{0}{1,2}. {2} ({3,-8}/{4,3}%/{5,2}D{6,-2})",
						Environment.NewLine,
						i + 1,
						capitalize ? Capitalize(a.Name.PadTRight(31, ' ')) : a.Name.PadTRight(31, ' '),
						weapon.Name,
						ac.Field5,
						ac.Field7,
						ac.Field8);
				}
				else
				{
					buf.AppendFormat("{0}{1,2}. {2}",
						Environment.NewLine,
						i + 1,
						capitalize ? Capitalize(x.Name) : x.Name);
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid)
		{
			StringBuilder buf01, buf02, srcBuf, dstBuf;
			MatchCollection matches;
			long numStars, numAts;
			long m, currUid;
			IEffect effect;
			Func<string> func;
			RetCode rc;
			int i;

			if (str == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			invalidUid = 0;

			if (str.Length < 4)
			{
				buf.Append(str);

				goto Cleanup;
			}

			if ((str[0] == '*' || (resolveFuncs && str[0] == '@')) && Char.IsDigit(str[1]) && Char.IsDigit(str[2]) && Char.IsDigit(str[3]) && long.TryParse(str.Substring(1, 3), out currUid) == true)
			{
				if (str[0] == '*')
				{
					effect = Globals.EDB[currUid];

					if (effect != null)
					{
						str = " " + str;
					}
				}
				else
				{
					if (MacroFuncs.TryGetValue(currUid, out func) && func != null)
					{
						str = " " + str;
					}
				}
			}

			matches = Regex.Matches(str, resolveFuncs ? Constants.ResolveUidMacroRegexPattern : Constants.ResolveEffectRegexPattern);

			if (matches.Count == 0)
			{
				buf.Append(str);

				goto Cleanup;
			}

			buf01 = new StringBuilder(Constants.BufSize);

			buf02 = new StringBuilder(Constants.BufSize);

			buf01.Append(str);

			srcBuf = buf01;

			dstBuf = buf02;

			m = 0;

			do
			{
				dstBuf.Clear();

				i = 0;

				foreach (Match match in matches)
				{
					foreach (Capture capture in match.Captures)
					{
						effect = null;

						func = null;

						numStars = 0;

						numAts = 0;

						if (capture.Value[1] == '*')
						{
							numStars = 1 + (capture.Value[0] == '*' ? 1 : 0);
						}
						else
						{
							numAts = 1 + (capture.Value[0] == '@' ? 1 : 0);
						}

						if (long.TryParse(capture.Value.Substring(2), out currUid) == false || currUid < 0)
						{
							currUid = 0;
						}

						if (numStars > 0)
						{
							effect = Globals.EDB[currUid];
						}
						else
						{
							if (MacroFuncs.TryGetValue(currUid, out func) == false)
							{
								func = null;
							}
						}

						dstBuf.Append(srcBuf.ToString().Substring(i, (capture.Index + (numStars == 1 || numAts == 1 ? 1 : 0)) - i));

						if (numStars > 0)
						{
							if (effect != null)
							{
								dstBuf.AppendFormat("{0}{1}{2}",
									numStars == 1 ? Environment.NewLine : "",
									numStars == 1 ? Environment.NewLine : "",
									effect.Desc);
							}
							else
							{
								if (invalidUid == 0 || (currUid > 0 && currUid < invalidUid))
								{
									invalidUid = currUid;
								}

								dstBuf.Append(capture.Value.Substring(numStars == 1 ? 1 : 0));
							}
						}
						else
						{
							if (func != null)
							{
								var desc = func();

								dstBuf.AppendFormat("{0}{1}{2}",
									numAts == 1 && !string.IsNullOrWhiteSpace(desc) ? Environment.NewLine : "",
									numAts == 1 && !string.IsNullOrWhiteSpace(desc) ? Environment.NewLine : "",
									desc);
							}
							else
							{
								dstBuf.Append(capture.Value.Substring(numAts == 1 ? 1 : 0));
							}
						}

						i = capture.Index + capture.Length;
					}
				}

				dstBuf.Append(srcBuf.ToString().Substring(i));

				if (++m >= Constants.MaxRecursionLevel)
				{
					recurse = false;
				}

				if (recurse)
				{
					matches = Regex.Matches(dstBuf.ToString(), resolveFuncs ? Constants.ResolveUidMacroRegexPattern : Constants.ResolveEffectRegexPattern);

					if (matches.Count > 0)
					{
						if (srcBuf == buf01)
						{
							srcBuf = buf02;

							dstBuf = buf01;
						}
						else
						{
							srcBuf = buf01;

							dstBuf = buf02;
						}
					}
				}
			}
			while (recurse && matches.Count > 0);

			buf.Append(dstBuf);

		Cleanup:

			return rc;
		}

		public virtual RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse)
		{
			long invalidUid = 0;

			return ResolveUidMacros(str, buf, resolveFuncs, recurse, ref invalidUid);
		}

		public virtual double GetWeaponPriceOrValue(string name, long complexity, Enums.Weapon type, long dice, long sides, bool calcPrice, ref bool isMarcosWeapon)
		{
			double wp;

			wp = 0.0;

			isMarcosWeapon = false;

			if (!string.IsNullOrWhiteSpace(name))
			{
				name = name.Trim().TrimEnd('#');
			}

			if (string.IsNullOrWhiteSpace(name) || !Enum.IsDefined(typeof(Enums.Weapon), type) || dice < 1 || sides < 1)
			{
				// PrintError

				goto Cleanup;
			}

			var weapon = GetWeapons(type);

			Debug.Assert(weapon != null);

			wp = weapon.MarcosPrice;

			if (complexity >= 0 && complexity < 10)
			{
				wp *= 0.80;
			}
			else if (complexity < 0)
			{
				wp *= 0.60;
			}

			isMarcosWeapon = string.Equals(name, weapon.MarcosName ?? weapon.Name, StringComparison.OrdinalIgnoreCase) && (complexity == -10 || complexity == 0 || complexity == 10) && dice == weapon.MarcosDice && sides == weapon.MarcosSides;

			if (!isMarcosWeapon)
			{
				if (complexity > 10)
				{
					wp += (calcPrice ? 40 : 5);
				}

				if (complexity > 15)
				{
					wp += (calcPrice ? 80 : 10);
				}

				if (complexity > 25)
				{
					wp += (calcPrice ? 320 : 20);
				}

				if (complexity > 35)
				{
					wp += (calcPrice ? 960 : 50);
				}

				if (dice * sides > 10)
				{
					wp += (calcPrice ? 40 : 5);
				}

				if (dice * sides > 15)
				{
					wp += (calcPrice ? 80 : 10);
				}

				if (dice * sides > 25)
				{
					wp += (calcPrice ? 320 : 20);
				}

				if (dice * sides > 35)
				{
					wp += (calcPrice ? 960 : 50);
				}
			}

		Cleanup:

			return wp;
		}

		public virtual double GetWeaponPriceOrValue(Classes.ICharacterWeapon weapon, bool calcPrice, ref bool isMarcosWeapon)
		{
			Debug.Assert(weapon != null);

			return GetWeaponPriceOrValue(weapon.Name, weapon.Complexity, weapon.Type, weapon.Dice, weapon.Sides, calcPrice, ref isMarcosWeapon);
		}

		public virtual double GetArmorPriceOrValue(Enums.Armor armor, bool calcPrice, ref bool isMarcosArmor)
		{
			double ap;

			ap = 0.0;

			isMarcosArmor = false;

			if (!Enum.IsDefined(typeof(Enums.Armor), armor))
			{
				// PrintError

				goto Cleanup;
			}

			var armor01 = ((long)armor / 2) * 2;

			if (armor01 > 0)
			{
				var armor02 = GetArmors((Enums.Armor)armor01);

				Debug.Assert(armor02 != null);

				if (armor02.MarcosPrice > 0)
				{
					if (calcPrice)
					{
						ap = armor02.MarcosPrice;
					}
					else
					{
						ap = armor02.ArtifactValue;
					}

					isMarcosArmor = true;
				}
				else
				{
					if (calcPrice)
					{
						armor02 = GetArmors(Enums.Armor.PlateMail);

						Debug.Assert(armor02 != null);

						ap = armor02.MarcosPrice + (((armor01 - (long)Enums.Armor.PlateMail) / 2) * 1000);
					}
					else
					{
						ap = armor02.ArtifactValue;
					}
				}
			}

		Cleanup:

			return ap;
		}

		public virtual void AppendFieldDesc(IPrintDescArgs args, StringBuilder fullDesc, StringBuilder briefDesc)
		{
			AppendFieldDesc(args, fullDesc != null ? fullDesc.ToString() : null, briefDesc != null ? briefDesc.ToString() : null);
		}

		public virtual void AppendFieldDesc(IPrintDescArgs args, string fullDesc, string briefDesc)
		{
			Debug.Assert(args != null && args.Buf != null && fullDesc != null);

			if (briefDesc != null)
			{
				if (args.FieldDesc == Enums.FieldDesc.Full)
				{
					args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, fullDesc, briefDesc);
				}
				else if (args.FieldDesc == Enums.FieldDesc.Brief)
				{
					args.Buf.AppendFormat("{0}{1}{0}", Environment.NewLine, briefDesc);
				}
			}
			else
			{
				if (args.FieldDesc == Enums.FieldDesc.Full)
				{
					args.Buf.AppendFormat("{0}{1}{0}", Environment.NewLine, fullDesc);
				}
			}
		}

		public virtual void CopyArtifactClassFields(Classes.IArtifactClass destAc, Classes.IArtifactClass sourceAc)
		{
			Debug.Assert(destAc != null);

			Debug.Assert(sourceAc != null);

			destAc.Field5 = sourceAc.Field5;

			destAc.Field6 = sourceAc.Field6;

			destAc.Field7 = sourceAc.Field7;

			destAc.Field8 = sourceAc.Field8;
		}

		public virtual IList<IArtifact> GetArtifactList(Func<bool> shouldQueryFunc, params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(shouldQueryFunc != null);

			Debug.Assert(whereClauseFuncs != null);

			var artifactList = new List<IArtifact>();

			if (shouldQueryFunc())
			{
				foreach (var whereClauseFunc in whereClauseFuncs)
				{
					Debug.Assert(whereClauseFunc != null);

					artifactList.AddRange(Globals.Database.ArtifactTable.Records.Where(whereClauseFunc));
				}
			}

			return artifactList;
		}

		public virtual IList<IMonster> GetMonsterList(Func<bool> shouldQueryFunc, params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(shouldQueryFunc != null);

			Debug.Assert(whereClauseFuncs != null);

			var monsterList = new List<IMonster>();

			if (shouldQueryFunc())
			{
				foreach (var whereClauseFunc in whereClauseFuncs)
				{
					Debug.Assert(whereClauseFunc != null);

					monsterList.AddRange(Globals.Database.MonsterTable.Records.Where(whereClauseFunc));
				}
			}

			return monsterList;
		}

		public virtual IList<IHaveListedName> GetRecordList(Func<bool> shouldQueryFunc, params Func<IHaveListedName, bool>[] whereClauseFuncs)
		{
			Debug.Assert(shouldQueryFunc != null);

			Debug.Assert(whereClauseFuncs != null);

			var recordList = new List<IHaveListedName>();

			if (shouldQueryFunc())
			{
				foreach (var whereClauseFunc in whereClauseFuncs)
				{
					Debug.Assert(whereClauseFunc != null);

					recordList.AddRange(Globals.Database.ArtifactTable.Records.Where(whereClauseFunc));

					recordList.AddRange(Globals.Database.MonsterTable.Records.Where(whereClauseFunc));
				}
			}

			return recordList;
		}

		public virtual IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc)
		{
			Debug.Assert(artifactList != null);

			Debug.Assert(which > 0);
			
			Debug.Assert(whereClauseFunc != null);

			return artifactList.Where(a => whereClauseFunc(a)).OrderBy(a => a.Uid).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc)
		{
			Debug.Assert(monsterList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return monsterList.Where(m => whereClauseFunc(m)).OrderBy(m => m.Uid).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual IHaveListedName GetNthRecord(IList<IHaveListedName> recordList, long which, Func<IHaveListedName, bool> whereClauseFunc)
		{
			Debug.Assert(recordList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return recordList.Where(x => whereClauseFunc(x)).OrderBy((x) =>
			{
				var ihu = x as IHaveUid;

				return string.Format("{0}_{1}", x.Name.ToLower(), ihu != null ? ihu.Uid : 0);

			}).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual void StripPoundCharsFromRecordNames(IList<IHaveListedName> recordList)
		{
			Debug.Assert(recordList != null);

			var sz = recordList.Count();

			for (var i = 0; i < sz; i++)
			{
				recordList[i].Name = recordList[i].Name.TrimEnd('#');
			}
		}

		public virtual void AddPoundCharsToRecordNames(IList<IHaveListedName> recordList)
		{
			long c;

			Debug.Assert(recordList != null);

			var sz = recordList.Count();

			do
			{
				c = 0;

				for (var i = 0; i < sz; i++)
				{
					for (var j = i + 1; j < sz; j++)
					{
						if (string.Equals(recordList[j].Name, recordList[i].Name, StringComparison.OrdinalIgnoreCase))
						{
							recordList[j].Name += "#";

							c = 1;
						}
					}
				}
			}
			while (c == 1);
		}

		public virtual void ConvertWeaponToTreasure(IArtifact artifact)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			if (artifact.IsWeapon01())
			{
				var ac = artifact.GetArtifactClass(Enums.ArtifactType.Treasure);

				if (ac == null)
				{
					ac = artifact.GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

					Debug.Assert(ac != null);

					ac.Type = Enums.ArtifactType.Treasure;

					ac.Field5 = 0;

					ac.Field6 = 0;

					ac.Field7 = 0;

					ac.Field8 = 0;
				}
				else
				{
					var acList = artifact.Classes.Where(ac01 => ac01 != null && !ac01.IsWeapon01()).ToList();

					for (var i = 0; i < artifact.Classes.Length; i++)
					{
						artifact.SetClasses(i, acList.Count > i ? acList[i] : null);
					}
				}

				rc = artifact.RemoveStateDesc(ReadyWeaponDesc);

				Debug.Assert(IsSuccess(rc));

				var monster = Globals.Database.MonsterTable.Records.FirstOrDefault(m => m.Weapon == artifact.Uid || m.Weapon == -artifact.Uid - 1);

				if (monster != null)
				{
					monster.Weapon = -1;
				}
			}
		}

		#endregion

		#region Class Engine

		public Engine()
		{
			Rand = new Random();

			NumberStrings = new string[]
			{
				"zero",
				"one",
				"two",
				"three",
				"four",
				"five",
				"six",
				"seven",
				"eight",
				"nine",
				"ten"
			};

			FieldDescNames = new string[]
			{
				"None",
				"Brief",
				"Full"
			};
			
			StatusNames = new string[]
			{
				"Unknown",
				"Alive",
				"Dead",
				"Adventuring"
			};

			ClothingNames = new string[]
			{
				"Armor & Shields",
				"Overclothes",
				"Shoes & Boots",
				"Gloves",
				"Hats & Headwear",
				"Jewelry",
				"Undergarments",
				"Shirts",
				"Pants"
			};


			CombatCodeDescs = new string[]
			{
				"Doesn't fight",
				"Uses weapons or natural weapons",		// "Will use wep. or nat. weapons", 
				"Normal",
				"Uses 'attacks' only"						// "'ATTACKS' only"
			};

			LightLevelNames = new string[]
			{
				"Dark",
				"Light"
			};

			Stats = new Classes.IStat[]
			{
				Globals.CreateInstance<Classes.IStat>(x =>
				{
					x.Name = "Intellect";
					x.Abbr = "IN";
					x.EmptyVal = "14";
					x.MinValue = 1;
					x.MaxValue = 24;
				}),
				Globals.CreateInstance<Classes.IStat>(x =>
				{
					x.Name = "Hardiness";
					x.Abbr = "HD";
					x.EmptyVal = "16";
					x.MinValue = 1;
					x.MaxValue = 300;
				}),
				Globals.CreateInstance<Classes.IStat>(x =>
				{
					x.Name = "Agility";
					x.Abbr = "AG";
					x.EmptyVal = "18";
					x.MinValue = 1;
					x.MaxValue = 30;
				}),
				Globals.CreateInstance<Classes.IStat>(x =>
				{
					x.Name = "Charisma";
					x.Abbr = "CH";
					x.EmptyVal = "16";
					x.MinValue = 1;
					x.MaxValue = 24;
				})
			};

			Spells = new Classes.ISpell[]
			{
				Globals.CreateInstance<Classes.ISpell>(x =>
				{
					x.Name = "Blast";
					x.HokasName = null;
					x.HokasPrice = Constants.BlastPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				Globals.CreateInstance<Classes.ISpell>(x =>
				{
					x.Name = "Heal";
					x.HokasName = null;
					x.HokasPrice = Constants.HealPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				Globals.CreateInstance<Classes.ISpell>(x =>
				{
					x.Name = "Speed";
					x.HokasName = null;
					x.HokasPrice = Constants.SpeedPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				Globals.CreateInstance<Classes.ISpell>(x =>
				{
					x.Name = "Power";
					x.HokasName = null;
					x.HokasPrice = Constants.PowerPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				})
			};

			Weapons = new Classes.IWeapon[]
			{
				Globals.CreateInstance<Classes.IWeapon>(x =>
				{
					x.Name = "Axe";
					x.EmptyVal = "5";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = Enums.PluralType.S;
					x.MarcosArticleType = Enums.ArticleType.An;
					x.MarcosPrice = Constants.AxePrice;
					x.MarcosDice = 1;
					x.MarcosSides = 6;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<Classes.IWeapon>(x =>
				{
					x.Name = "Bow";
					x.EmptyVal = "-10";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = Enums.PluralType.S;
					x.MarcosArticleType = Enums.ArticleType.A;
					x.MarcosPrice = Constants.BowPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 6;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<Classes.IWeapon>(x =>
				{
					x.Name = "Club";
					x.EmptyVal = "20";
					x.MarcosName = "Mace";
					x.MarcosIsPlural = false;
					x.MarcosPluralType = Enums.PluralType.S;
					x.MarcosArticleType = Enums.ArticleType.A;
					x.MarcosPrice = Constants.MacePrice;
					x.MarcosDice = 1;
					x.MarcosSides = 4;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<Classes.IWeapon>(x =>
				{
					x.Name = "Spear";
					x.EmptyVal = "10";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = Enums.PluralType.S;
					x.MarcosArticleType = Enums.ArticleType.A;
					x.MarcosPrice = Constants.SpearPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 5;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<Classes.IWeapon>(x =>
				{
					x.Name = "Sword";
					x.EmptyVal = "0";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = Enums.PluralType.S;
					x.MarcosArticleType = Enums.ArticleType.A;
					x.MarcosPrice = Constants.SwordPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 8;
					x.MinValue = -50;
					x.MaxValue = 122;
				})
			};

			Armors = new Classes.IArmor[]
			{
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Skin/Clothes";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Clothes & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Leather";
					x.MarcosName = "Leather Armor";
					x.MarcosPrice = Constants.LeatherArmorPrice;
					x.MarcosNum = 1;
					x.ArtifactValue = 100;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Leather & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Chain Mail";
					x.MarcosName = null;
					x.MarcosPrice = Constants.ChainMailPrice;
					x.MarcosNum = 2;
					x.ArtifactValue = 200;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Chain Mail & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Plate Mail";
					x.MarcosName = null;
					x.MarcosPrice = Constants.PlateMailPrice;
					x.MarcosNum = 3;
					x.ArtifactValue = 350;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Plate Mail & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 500;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 650;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 800;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 950;
				}),
				Globals.CreateInstance<Classes.IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				})
			};

			Genders = new Classes.IGender[]
			{
				Globals.CreateInstance<Classes.IGender>(x =>
				{
					x.Name = "Male";
					x.MightyDesc = "Mighty";
					x.BoyDesc = "boy";
					x.HimDesc = "him";
					x.HisDesc = "his";
					x.HeDesc = "he";
				}),
				Globals.CreateInstance<Classes.IGender>(x =>
				{
					x.Name = "Female";
					x.MightyDesc = "Fair";
					x.BoyDesc = "girl";
					x.HimDesc = "her";
					x.HisDesc = "her";
					x.HeDesc = "she";
				}),
				Globals.CreateInstance<Classes.IGender>(x =>
				{
					x.Name = "Neutral";
					x.MightyDesc = "Androgynous";
					x.BoyDesc = null;
					x.HimDesc = "it";
					x.HisDesc = "its";
					x.HeDesc = "it";
				})
			};

			Directions = new Classes.IDirection[]
			{
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "North";
					x.PrintedName = "North";
					x.Abbr = "N";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "South";
					x.PrintedName = "South";
					x.Abbr = "S";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "East";
					x.PrintedName = "East";
					x.Abbr = "E";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "West";
					x.PrintedName = "West";
					x.Abbr = "W";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "Up";
					x.PrintedName = "Up";
					x.Abbr = "U";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "Down";
					x.PrintedName = "Down";
					x.Abbr = "D";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "Northeast";
					x.PrintedName = "NE";
					x.Abbr = "NE";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "Northwest";
					x.PrintedName = "NW";
					x.Abbr = "NW";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "Southeast";
					x.PrintedName = "SE";
					x.Abbr = "SE";
				}),
				Globals.CreateInstance<Classes.IDirection>(x =>
				{
					x.Name = "Southwest";
					x.PrintedName = "SW";
					x.Abbr = "SW";
				})
			};

			Friendlinesses = new Classes.IFriendliness[]
			{
				Globals.CreateInstance<Classes.IFriendliness>(x =>
				{
					x.Name = "Enemy";
					x.SmileDesc = "growl";
				}),
				Globals.CreateInstance<Classes.IFriendliness>(x =>
				{
					x.Name = "Neutral";
					x.SmileDesc = "ignore";
				}),
				Globals.CreateInstance<Classes.IFriendliness>(x =>
				{
					x.Name = "Friend";
					x.SmileDesc = "smile";
				})
			};

			RoomTypes = new Classes.IRoomType[]
			{
				Globals.CreateInstance<Classes.IRoomType>(x =>
				{
					x.Name = "Indoors";
					x.RoomDesc = "room";
					x.ExitDesc = "exits";
					x.FleeDesc = "an exit";
				}),
				Globals.CreateInstance<Classes.IRoomType>(x =>
				{
					x.Name = "Outdoors";
					x.RoomDesc = "area";
					x.ExitDesc = "paths";
					x.FleeDesc = "a path";
				})
			};

			ArtifactTypes = new Classes.IArtifactType[]
			{
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Gold";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Treasure";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Weapon";
					x.WeightEmptyVal = "25";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Complexity";
					x.Field5EmptyVal = "5";
					x.Field6Name = "Wpn Type";
					x.Field6EmptyVal = "5";
					x.Field7Name = "Dice";
					x.Field7EmptyVal = "1";
					x.Field8Name = "Sides";
					x.Field8EmptyVal = "6";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Magic Weapon";
					x.WeightEmptyVal = "25";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Complexity";
					x.Field5EmptyVal = "15";
					x.Field6Name = "Wpn Type";
					x.Field6EmptyVal = "5";
					x.Field7Name = "Dice";
					x.Field7EmptyVal = "1";
					x.Field8Name = "Sides";
					x.Field8EmptyVal = "10";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Key Uid";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Open/Closed";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Max Weight Inside";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Max Items Inside";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Light Source";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Counter";
					x.Field5EmptyVal = "999";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Drinkable";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Heal Amount";
					x.Field5EmptyVal = "10";
					x.Field6Name = "Number Of Uses";
					x.Field6EmptyVal = "3";
					x.Field7Name = "Open/Closed";
					x.Field7EmptyVal = "1";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Readable";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Effect #1";
					x.Field5EmptyVal = "1";
					x.Field6Name = "Number Of Effects";
					x.Field6EmptyVal = "1";
					x.Field7Name = "Open/Closed";
					x.Field7EmptyVal = "1";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Door/Gate";
					x.WeightEmptyVal = "-999";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Room Uid Beyond";
					x.Field5EmptyVal = "1";
					x.Field6Name = "Key Uid";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Open/Closed";
					x.Field7EmptyVal = "1";
					x.Field8Name = "Hidden";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Edible";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Heal Amount";
					x.Field5EmptyVal = "10";
					x.Field6Name = "Number Of Uses";
					x.Field6EmptyVal = "4";
					x.Field7Name = "Open/Closed";
					x.Field7EmptyVal = "1";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Bound Monster";
					x.WeightEmptyVal = "999";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Monster Uid";
					x.Field5EmptyVal = "1";
					x.Field6Name = "Key Uid";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Guard Uid";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Wearable";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Armor Class";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Clothing Type";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Disguised Monster";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Monster Uid";
					x.Field5EmptyVal = "1";
					x.Field6Name = "Effect #1";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Number Of Effects";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "Dead Body";
					x.WeightEmptyVal = "150";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Can Take";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "User Defined #1";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "User Defined #2";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				}),
				Globals.CreateInstance<Classes.IArtifactType>(x =>
				{
					x.Name = "User Defined #3";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
					x.Field6Name = "Field6";
					x.Field6EmptyVal = "0";
					x.Field7Name = "Field7";
					x.Field7EmptyVal = "0";
					x.Field8Name = "Field8";
					x.Field8EmptyVal = "0";
				})
			};

			AttackDescs = new Dictionary<Tuple<Enums.Weapon, long>, string>()
			{
				{ new Tuple<Enums.Weapon, long>(0, 1), "lunge{0} at" },
				{ new Tuple<Enums.Weapon, long>(0, 2), "tear{0} at" },
				{ new Tuple<Enums.Weapon, long>(0, 3), "claw{0} at" },
				{ new Tuple<Enums.Weapon, long>(0, 4), "charge{0} at" },
				{ new Tuple<Enums.Weapon, long>(0, 5), "punche{0} at" },
				{ new Tuple<Enums.Weapon, long>(0, 6), "kick{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 1), "swing{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 2), "chop{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 3), "swing{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Bow, 1), "shoot{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Bow, 2), "shoot{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Bow, 3), "shoot{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 1), "swing{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 2), "swing{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 3), "swing{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 1), "stab{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 2), "lunge{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 3), "jab{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Sword, 1), "swing{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Sword, 2), "chop{0} at" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Sword, 3), "stab{0} at" }
			};

			MissDescs = new Dictionary<Tuple<Enums.Weapon, long>, string>()
			{
				{ new Tuple<Enums.Weapon, long>(0, 1), "Missed" },
				{ new Tuple<Enums.Weapon, long>(0, 2), "Missed" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 1), "Dodged" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 2), "Missed" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Bow, 1), "Missed" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Bow, 2), "Missed" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 1), "Dodged" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 2), "Missed" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 1), "Dodged" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 2), "Missed" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Sword, 1), "Parried" },
				{ new Tuple<Enums.Weapon, long>(Enums.Weapon.Sword, 2), "Missed" },
			};

			MacroFuncs = new Dictionary<long, Func<string>>();
			
			Preps = new string[]
			{
				"to",
				"on",
				"in",
				"at",
				"from",
				"with",
				"onto",
				"into",
				"inside"
			};

			Articles = new string[]
			{
				"a",
				"an",
				"some",
				"the",
				"this",
				"these",
				"that",
				"those",
				"my",
				"your",
				"his",
				"her",
				"its"
			};

			NumCacheItems = Constants.NumCacheItems;

			ProvidingLightDesc = Constants.ProvidingLightDesc;

			ReadyWeaponDesc = Constants.ReadyWeaponDesc;

			BrokenDesc = Constants.BrokenDesc;

			EmptyDesc = Constants.EmptyDesc;

			BlastDesc = Constants.BlastDesc;

			UnknownName = "???";
		}

		#endregion

		#endregion
	}
}
