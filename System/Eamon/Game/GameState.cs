
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using Eamon.Game.Validation;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class GameState : Validator, IGameState
	{
		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

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

		public virtual long UsedWpnIdx { get; set; }

		public virtual long[] NBTL { get; set; }

		public virtual long[] DTTL { get; set; }

		public virtual long[] Sa { get; set; }

		public virtual long[] HeldWpnUids { get; set; }

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
				Globals.Database.FreeGameStateUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IValidator

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Uid > 0;
		}

		protected virtual bool ValidateAr(IField field, IValidateArgs args)
		{
			return Ar >= 0;
		}

		protected virtual bool ValidateCm(IField field, IValidateArgs args)
		{
			return Cm > 0;
		}

		protected virtual bool ValidateLs(IField field, IValidateArgs args)
		{
			return Ls >= 0;
		}

		protected virtual bool ValidateSh(IField field, IValidateArgs args)
		{
			return Sh >= 0;
		}

		protected virtual bool ValidateLt(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.LightLevel), Lt);
		}

		protected virtual bool ValidateSpeed(IField field, IValidateArgs args)
		{
			return Speed >= 0;
		}

		protected virtual bool ValidateWt(IField field, IValidateArgs args)
		{
			return Wt >= 0;
		}

		protected virtual bool ValidateCurrTurn(IField field, IValidateArgs args)
		{
			return CurrTurn >= 0;
		}

		protected virtual bool ValidateUsedWpnIdx(IField field, IValidateArgs args)
		{
			return UsedWpnIdx >= 0 && UsedWpnIdx < HeldWpnUids.Length;
		}

		protected virtual bool ValidateNBTL(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(Enum.IsDefined(typeof(Enums.Friendliness), i));

			return GetNBTL(i) >= 0;
		}

		protected virtual bool ValidateDTTL(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(Enum.IsDefined(typeof(Enums.Friendliness), i));

			return GetDTTL(i) >= 0;
		}

		protected virtual bool ValidateSa(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return GetSa(i) >= spell.MinValue && GetSa(i) <= spell.MaxValue;
		}

		protected virtual bool ValidateHeldWpnUids(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < HeldWpnUids.Length);

			return GetHeldWpnUids(i) >= 0;
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

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
						x.GetValue = () => Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetValue = () => IsUidRecycled;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ar";
						x.Validate = ValidateAr;
						x.GetValue = () => Ar;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Cm";
						x.Validate = ValidateCm;
						x.GetValue = () => Cm;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ls";
						x.Validate = ValidateLs;
						x.GetValue = () => Ls;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ro";
						x.GetValue = () => Ro;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "R2";
						x.GetValue = () => R2;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "R3";
						x.GetValue = () => R3;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Sh";
						x.Validate = ValidateSh;
						x.GetValue = () => Sh;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Af";
						x.GetValue = () => Af;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Die";
						x.GetValue = () => Die;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Lt";
						x.Validate = ValidateLt;
						x.GetValue = () => Lt;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Speed";
						x.Validate = ValidateSpeed;
						x.GetValue = () => Speed;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Wt";
						x.Validate = ValidateWt;
						x.GetValue = () => Wt;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Vr";
						x.GetValue = () => Vr;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Vm";
						x.GetValue = () => Vm;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Va";
						x.GetValue = () => Va;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CurrTurn";
						x.Validate = ValidateCurrTurn;
						x.GetValue = () => CurrTurn;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "UsedWpnIdx";
						x.Validate = ValidateUsedWpnIdx;
						x.GetValue = () => UsedWpnIdx;
					})
				};

				var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

				foreach (var fv in friendlinessValues)
				{
					var i = (long)fv;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("NBTL[{0}]", i);
							x.UserData = i;
							x.Validate = ValidateNBTL;
							x.GetValue = () => GetNBTL(i);
						})
					);

					if (Globals.IsRulesetVersion(5))
					{
						Fields.Add
						(
							Globals.CreateInstance<IField>(x =>
							{
								x.Name = string.Format("DTTL[{0}]", i);
								x.UserData = i;
								x.Validate = ValidateDTTL;
								x.GetValue = () => GetDTTL(i);
							})
						);
					}
				}

				var spellValues = EnumUtil.GetValues<Enums.Spell>();

				foreach (var sv in spellValues)
				{
					var i = (long)sv;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Sa[{0}]", i);
							x.UserData = i;
							x.Validate = ValidateSa;
							x.GetValue = () => GetSa(i);
						})
					);
				}

				for (var i = 0; i < HeldWpnUids.Length; i++)
				{
					var j = i;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("HeldWpnUids[{0}]", j);
							x.UserData = j;
							x.Validate = ValidateHeldWpnUids;
							x.GetValue = () => GetHeldWpnUids(j);
						})
					);
				}
			}

			return Fields;
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
			IsUidRecycled = true;

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
