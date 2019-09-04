
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IGameState : IGameBase, IComparable<IGameState>
	{
		#region Properties

		/// <summary></summary>
		long Ar { get; set; }

		/// <summary></summary>
		long Cm { get; set; }

		/// <summary></summary>
		long Ls { get; set; }

		/// <summary></summary>
		long Ro { get; set; }

		/// <summary></summary>
		long R2 { get; set; }

		/// <summary></summary>
		long R3 { get; set; }

		/// <summary></summary>
		long Sh { get; set; }

		/// <summary></summary>
		long Af { get; set; }

		/// <summary></summary>
		long Die { get; set; }

		/// <summary></summary>
		long Speed { get; set; }

		/// <summary></summary>
		bool Vr { get; set; }

		/// <summary></summary>
		bool Vm { get; set; }

		/// <summary></summary>
		bool Va { get; set; }

		/// <summary></summary>
		long CurrTurn { get; set; }

		/// <summary></summary>
		long PauseCombatMs { get; set; }

		/// <summary></summary>
		long UsedWpnIdx { get; set; }

		/// <summary></summary>
		long[] Sa { get; set; }

		/// <summary></summary>
		long[] HeldWpnUids { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetNBTL(long index);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		long GetNBTL(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetDTTL(long index);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		long GetDTTL(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetSa(long index);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		long GetSa(Spell spell);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetHeldWpnUids(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetSa(long index, long value);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <param name="value"></param>
		void SetSa(Spell spell, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetHeldWpnUids(long index, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void ModSa(long index, long value);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <param name="value"></param>
		void ModSa(Spell spell, long value);

		#endregion
	}
}
