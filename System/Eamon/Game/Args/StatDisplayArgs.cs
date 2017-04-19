
// StatDisplayArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class StatDisplayArgs : IStatDisplayArgs
	{
		public virtual IMonster Monster { get; set; }

		public virtual string ArmorString { get; set; }

		public virtual long[] SpellAbilities { get; set; }

		public virtual long Speed { get; set; }

		public virtual long CharmMon { get; set; }

		public virtual long Weight { get; set; }

		public virtual long GetSpellAbilities(long spell)
		{
			return SpellAbilities[spell];
		}

		public virtual long GetSpellAbilities(Enums.Spell spell)
		{
			return GetSpellAbilities((long)spell);
		}
	}
}
