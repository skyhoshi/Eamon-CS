
// IStatDisplayArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Args
{
	public interface IStatDisplayArgs
	{
		#region Properties

		IMonster Monster { get; set; }

		string ArmorString { get; set; }

		long[] SpellAbilities { get; set; }

		long Speed { get; set; }

		long CharmMon { get; set; }

		long Weight { get; set; }

		#endregion

		#region Methods

		long GetSpellAbilities(long spell);

		long GetSpellAbilities(Enums.Spell spell);

		#endregion
	}
}
