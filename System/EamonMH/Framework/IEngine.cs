
// IEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Classes = Eamon.Framework.Primitive.Classes;

namespace EamonMH.Framework
{
	public interface IEngine : Eamon.Framework.IEngine
	{
		bool IsCharDOrM(char ch);

		bool IsCharROrT(char ch);

		bool IsCharDOrIOrX(char ch);

		bool IsCharDOrWOrX(char ch);

		bool IsCharUOrCOrX(char ch);

		bool IsChar1OrX(char ch);

		bool IsChar1Or2OrX(char ch);

		bool IsCharTOrL(char ch);

		bool IsCharBOrSOrAOrX(char ch);

		bool IsCharGOrFOrPOrX(char ch);

		bool IsCharWpnType(char ch);

		bool IsCharWpnTypeOrX(char ch);

		bool IsCharSpellType(char ch);

		bool IsCharSpellTypeOrX(char ch);

		bool IsCharMarcosNumOrX(char ch);

		bool IsCharWpnNumOrX(char ch);

		bool IsCharStat(char ch);

		long GetMaxArmorMarcosNum();

		Classes.IArmor GetArmorByMarcosNum(long marcosNum);

		void MhProcessArgv(bool secondPass, ref bool nlFlag);
	};
}
