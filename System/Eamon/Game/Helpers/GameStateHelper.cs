
// GameStateHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IGameState>))]
	public class GameStateHelper : Helper<IGameState>
	{
		#region Protected Methods

		#region Interface IHelper

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateAr(IField field, IValidateArgs args)
		{
			return Record.Ar >= 0;
		}

		protected virtual bool ValidateCm(IField field, IValidateArgs args)
		{
			return Record.Cm > 0;
		}

		protected virtual bool ValidateLs(IField field, IValidateArgs args)
		{
			return Record.Ls >= 0;
		}

		protected virtual bool ValidateSh(IField field, IValidateArgs args)
		{
			return Record.Sh >= 0;
		}

		protected virtual bool ValidateLt(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.LightLevel), Record.Lt);
		}

		protected virtual bool ValidateSpeed(IField field, IValidateArgs args)
		{
			return Record.Speed >= 0;
		}

		protected virtual bool ValidateWt(IField field, IValidateArgs args)
		{
			return Record.Wt >= 0;
		}

		protected virtual bool ValidateCurrTurn(IField field, IValidateArgs args)
		{
			return Record.CurrTurn >= 0;
		}

		protected virtual bool ValidateUsedWpnIdx(IField field, IValidateArgs args)
		{
			return Record.UsedWpnIdx >= 0 && Record.UsedWpnIdx < Record.HeldWpnUids.Length;
		}

		protected virtual bool ValidateNBTL(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(Enum.IsDefined(typeof(Enums.Friendliness), i));

			return Record.GetNBTL(i) >= 0;
		}

		protected virtual bool ValidateDTTL(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(Enum.IsDefined(typeof(Enums.Friendliness), i));

			return Record.GetDTTL(i) >= 0;
		}

		protected virtual bool ValidateSa(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSa(i) >= spell.MinValue && Record.GetSa(i) <= spell.MaxValue;
		}

		protected virtual bool ValidateHeldWpnUids(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < Record.HeldWpnUids.Length);

			return Record.GetHeldWpnUids(i) >= 0;
		}

		#endregion

		protected override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Uid";
						x.Validate = ValidateUid;
						x.GetValue = () => Record.Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetValue = () => Record.IsUidRecycled;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ar";
						x.Validate = ValidateAr;
						x.GetValue = () => Record.Ar;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Cm";
						x.Validate = ValidateCm;
						x.GetValue = () => Record.Cm;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ls";
						x.Validate = ValidateLs;
						x.GetValue = () => Record.Ls;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ro";
						x.GetValue = () => Record.Ro;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "R2";
						x.GetValue = () => Record.R2;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "R3";
						x.GetValue = () => Record.R3;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Sh";
						x.Validate = ValidateSh;
						x.GetValue = () => Record.Sh;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Af";
						x.GetValue = () => Record.Af;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Die";
						x.GetValue = () => Record.Die;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Lt";
						x.Validate = ValidateLt;
						x.GetValue = () => Record.Lt;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Speed";
						x.Validate = ValidateSpeed;
						x.GetValue = () => Record.Speed;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Wt";
						x.Validate = ValidateWt;
						x.GetValue = () => Record.Wt;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Vr";
						x.GetValue = () => Record.Vr;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Vm";
						x.GetValue = () => Record.Vm;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Va";
						x.GetValue = () => Record.Va;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CurrTurn";
						x.Validate = ValidateCurrTurn;
						x.GetValue = () => Record.CurrTurn;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "UsedWpnIdx";
						x.Validate = ValidateUsedWpnIdx;
						x.GetValue = () => Record.UsedWpnIdx;
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
							x.GetValue = () => Record.GetNBTL(i);
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
								x.GetValue = () => Record.GetDTTL(i);
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
							x.GetValue = () => Record.GetSa(i);
						})
					);
				}

				for (var i = 0; i < Record.HeldWpnUids.Length; i++)
				{
					var j = i;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("HeldWpnUids[{0}]", j);
							x.UserData = j;
							x.Validate = ValidateHeldWpnUids;
							x.GetValue = () => Record.GetHeldWpnUids(j);
						})
					);
				}
			}

			return Fields;
		}

		#endregion

		#region Class GameStateHelper

		protected virtual void SetGameStateUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetGameStateUid();

				Record.IsUidRecycled = true;
			}
			else if (!editRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Class GameStateHelper

		public GameStateHelper()
		{
			SetUidIfInvalid = SetGameStateUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
