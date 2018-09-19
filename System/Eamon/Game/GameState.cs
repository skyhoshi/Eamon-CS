
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class GameState : GameBase, IGameState
	{
		#region Public Properties

		#region Interface IGameState

		public virtual long Ar { get; set; }

		public virtual long Cm { get; set; }

		public virtual long Ls { get; set; }

		public virtual long Ro { get; set; }

		public virtual long R2 { get; set; }

		public virtual long R3 { get; set; }

		public virtual long Sh { get; set; }

		public virtual long Af { get; set; }

		public virtual long Die { get; set; }

		public virtual long Lt { get; set; }

		public virtual long Speed { get; set; }

		public virtual long Wt { get; set; }

		public virtual bool Vr { get; set; }

		public virtual bool Vm { get; set; }

		public virtual bool Va { get; set; }

		public virtual long CurrTurn { get; set; }

		public virtual long PauseCombatMs { get; set; }

		public virtual long UsedWpnIdx { get; set; }

		public virtual long[] NBTL { get; set; }

		public virtual long[] DTTL { get; set; }

		public virtual long[] Sa { get; set; }

		public virtual long[] HeldWpnUids { get; set; }

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
				Globals.Database.FreeGameStateUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IGameState gameState)
		{
			return this.Uid.CompareTo(gameState.Uid);
		}

		#endregion

		#region Interface IGameState

		public virtual long GetNBTL(long index)
		{
			return NBTL[index];
		}

		public virtual long GetNBTL(Enums.Friendliness friendliness)
		{
			return GetNBTL((long)friendliness);
		}

		public virtual long GetDTTL(long index)
		{
			return DTTL[index];
		}

		public virtual long GetDTTL(Enums.Friendliness friendliness)
		{
			return GetDTTL((long)friendliness);
		}

		public virtual long GetSa(long index)
		{
			return Sa[index];
		}

		public virtual long GetSa(Enums.Spell spell)
		{
			return GetSa((long)spell);
		}

		public virtual long GetHeldWpnUids(long index)
		{
			return HeldWpnUids[index];
		}

		public virtual void SetNBTL(long index, long value)
		{
			NBTL[index] = value;
		}

		public virtual void SetNBTL(Enums.Friendliness friendliness, long value)
		{
			SetNBTL((long)friendliness, value);
		}

		public virtual void SetDTTL(long index, long value)
		{
			DTTL[index] = value;
		}

		public virtual void SetDTTL(Enums.Friendliness friendliness, long value)
		{
			SetDTTL((long)friendliness, value);
		}

		public virtual void SetSa(long index, long value)
		{
			Sa[index] = value;
		}

		public virtual void SetSa(Enums.Spell spell, long value)
		{
			SetSa((long)spell, value);
		}

		public virtual void SetHeldWpnUids(long index, long value)
		{
			HeldWpnUids[index] = value;
		}

		public virtual void ModNBTL(long index, long value)
		{
			NBTL[index] += value;
		}

		public virtual void ModNBTL(Enums.Friendliness friendliness, long value)
		{
			ModNBTL((long)friendliness, value);
		}

		public virtual void ModDTTL(long index, long value)
		{
			DTTL[index] += value;
		}

		public virtual void ModDTTL(Enums.Friendliness friendliness, long value)
		{
			ModDTTL((long)friendliness, value);
		}

		public virtual void ModSa(long index, long value)
		{
			Sa[index] += value;
		}

		public virtual void ModSa(Enums.Spell spell, long value)
		{
			ModSa((long)spell, value);
		}

		#endregion

		#region Class GameState

		public GameState()
		{
			var character = Globals.CreateInstance<ICharacter>();

			Debug.Assert(character != null);

			var friendlinessesLength = (long)EnumUtil.GetLastValue<Enums.Friendliness>() + 1;

			NBTL = new long[friendlinessesLength];

			DTTL = new long[friendlinessesLength];

			Sa = new long[character.SpellAbilities.Length];

			HeldWpnUids = new long[character.Weapons.Length];
		}

		#endregion

		#endregion
	}
}
