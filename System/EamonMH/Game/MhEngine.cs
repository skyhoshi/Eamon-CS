
// MhEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonMH.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using Classes = Eamon.Framework.Primitive.Classes;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game
{
	[ClassMappings]
	public class MhEngine : IMhEngine
	{
		public virtual bool IsCharDOrM(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'D' || ch == 'M';
		}

		public virtual bool IsCharROrT(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'R' || ch == 'T';
		}

		public virtual bool IsCharDOrIOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'D' || ch == 'I' || ch == 'X';
		}

		public virtual bool IsCharDOrWOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'D' || ch == 'W' || ch == 'X';
		}

		public virtual bool IsCharUOrCOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'U' || ch == 'C' || ch == 'X';
		}

		public virtual bool IsChar1OrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == '1' || ch == 'X';
		}

		public virtual bool IsChar1Or2OrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == '1' || ch == '2' || ch == 'X';
		}

		public virtual bool IsCharTOrL(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'T' || ch == 'L';
		}

		public virtual bool IsCharBOrSOrAOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'B' || ch == 'S' || ch == 'A' || ch == 'X';
		}

		public virtual bool IsCharGOrFOrPOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'G' || ch == 'F' || ch == 'P' || ch == 'X';
		}

		public virtual bool IsCharWpnType(char ch)
		{
			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			ch = Char.ToUpper(ch);

			return ch >= ('0' + (long)weaponValues[0]) && ch <= ('0' + (long)weaponValues[weaponValues.Count - 1]);
		}

		public virtual bool IsCharWpnTypeOrX(char ch)
		{
			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			ch = Char.ToUpper(ch);

			return (ch >= ('0' + (long)weaponValues[0]) && ch <= ('0' + (long)weaponValues[weaponValues.Count - 1])) || ch == 'X';
		}

		public virtual bool IsCharSpellType(char ch)
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			ch = Char.ToUpper(ch);

			return ch >= ('0' + (long)spellValues[0]) && ch <= ('0' + (long)spellValues[spellValues.Count - 1]);
		}

		public virtual bool IsCharSpellTypeOrX(char ch)
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			ch = Char.ToUpper(ch);

			return (ch >= ('0' + (long)spellValues[0]) && ch <= ('0' + (long)spellValues[spellValues.Count - 1])) || ch == 'X';
		}

		public virtual bool IsCharMarcosNumOrX(char ch)
		{
			var i = GetMaxArmorMarcosNum();

			ch = Char.ToUpper(ch);

			return (ch >= '1' && ch <= ('1' + (i - 1))) || ch == 'X';
		}

		public virtual bool IsCharWpnNumOrX(char ch)
		{
			long i = 0;

			var rc = Globals.Character.GetWeaponCount(ref i);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			ch = Char.ToUpper(ch);

			return (ch >= '1' && ch <= ('1' + (i - 1))) || ch == 'X';
		}

		public virtual bool IsCharStat(char ch)
		{
			var statValues = EnumUtil.GetValues<Enums.Stat>();

			ch = Char.ToUpper(ch);

			return ch >= ('0' + (long)statValues[0]) && ch <= ('0' + (long)statValues[statValues.Count - 1]);
		}

		public virtual long GetMaxArmorMarcosNum()
		{
			long rc = 0;

			var armorValues = EnumUtil.GetValues<Enums.Armor>();

			for (var i = 0; i < armorValues.Count; i++)
			{
				var armor = Globals.Engine.GetArmors(armorValues[i]);

				Debug.Assert(armor != null);

				if (rc < armor.MarcosNum)
				{
					rc = armor.MarcosNum;
				}
			}

			return rc;
		}

		public virtual Classes.IArmor GetArmorByMarcosNum(long marcosNum)
		{
			Classes.IArmor rc = null;

			var armorValues = EnumUtil.GetValues<Enums.Armor>();

			for (var i = 0; i < armorValues.Count; i++)
			{
				var armor = Globals.Engine.GetArmors(armorValues[i]);

				Debug.Assert(armor != null);

				if (armor.MarcosNum == marcosNum)
				{
					rc = armor;

					break;
				}
			}

			return rc;
		}

		public virtual void ProcessArgv(bool secondPass, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Globals.Argv.Length; i++)
			{
				if (string.Equals(Globals.Argv[i], "--workingDirectory", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (string.Equals(Globals.Argv[i], "--ignoreMutex", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (string.Equals(Globals.Argv[i], "--configFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && !secondPass)
					{
						Globals.ConfigFileName = Globals.Argv[i].Trim();
					}
				}
				else if (string.Equals(Globals.Argv[i], "--filesetFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.MhFilesetFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--characterFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.MhCharacterFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--effectFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.MhEffectFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--characterName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-chrnm", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.CharacterName = Globals.Argv[i].Trim();
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
					}

					Globals.Out.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Globals.Argv[i]);

					nlFlag = true;
				}
			}
		}
	}
}
