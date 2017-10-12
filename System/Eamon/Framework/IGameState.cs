
// IGameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IGameState : IGameBase, IComparable<IGameState>
	{
		#region Properties

		long Ar { get; set; }

		long Cm { get; set; }

		long Ls { get; set; }

		long Ro { get; set; }

		long R2 { get; set; }

		long R3 { get; set; }

		long Sh { get; set; }

		long Af { get; set; }

		long Die { get; set; }

		long Lt { get; set; }

		long Speed { get; set; }

		long Wt { get; set; }

		bool Vr { get; set; }

		bool Vm { get; set; }

		bool Va { get; set; }

		long CurrTurn { get; set; }

		long UsedWpnIdx { get; set; }

		long[] NBTL { get; set; }

		long[] DTTL { get; set; }

		long[] Sa { get; set; }

		long[] HeldWpnUids { get; set; }

		#endregion

		#region Methods

		long GetNBTL(long index);

		long GetNBTL(Enums.Friendliness friendliness);

		long GetDTTL(long index);

		long GetDTTL(Enums.Friendliness friendliness);

		long GetSa(long index);

		long GetSa(Enums.Spell spell);

		long GetHeldWpnUids(long index);

		void SetNBTL(long index, long value);

		void SetNBTL(Enums.Friendliness friendliness, long value);

		void SetDTTL(long index, long value);

		void SetDTTL(Enums.Friendliness friendliness, long value);

		void SetSa(long index, long value);

		void SetSa(Enums.Spell spell, long value);

		void SetHeldWpnUids(long index, long value);

		void ModNBTL(long index, long value);

		void ModNBTL(Enums.Friendliness friendliness, long value);

		void ModDTTL(long index, long value);

		void ModDTTL(Enums.Friendliness friendliness, long value);

		void ModSa(long index, long value);

		void ModSa(Enums.Spell spell, long value);

		#endregion
	}
}
