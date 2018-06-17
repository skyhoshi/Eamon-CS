
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IGameState>))]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		protected virtual bool ValidateFl(IField field, IValidateArgs args)
		{
			return Record.Fl >= 0 && Record.Fl <= 2;
		}

		protected virtual bool ValidateLu(IField field, IValidateArgs args)
		{
			return Record.Lu >= 0 && Record.Lu <= 2;
		}

		protected virtual bool ValidateMc(IField field, IValidateArgs args)
		{
			return Record.Mc >= 0;
		}

		protected virtual bool ValidateMf(IField field, IValidateArgs args)
		{
			return Record.Mf >= 0 && Record.Mf <= 1;
		}

		protected virtual bool ValidateMa(IField field, IValidateArgs args)
		{
			return Record.Ma >= 0;
		}

		protected virtual bool ValidateMz(IField field, IValidateArgs args)
		{
			// TODO: apply proper validation range

			return true;
		}

		protected virtual bool ValidateTp(IField field, IValidateArgs args)
		{
			return Record.Tp >= 0 && Record.Tp <= 10;
		}

		protected virtual bool ValidateBy(IField field, IValidateArgs args)
		{
			return Record.By >= 0;
		}

		protected virtual bool ValidatePr(IField field, IValidateArgs args)
		{
			return Record.Pr >= 0 && Record.Pr <= 1;
		}

		protected virtual bool ValidateBt(IField field, IValidateArgs args)
		{
			return Record.Bt >= 0 && Record.Bt <= 2;
		}

		protected virtual bool ValidateTk(IField field, IValidateArgs args)
		{
			return Record.Tk >= 0 && Record.Tk <= 3;
		}

		protected virtual bool ValidateCw(IField field, IValidateArgs args)
		{
			return Record.Cw >= 0 && Record.Cw <= 1;
		}

		protected virtual bool ValidateLh(IField field, IValidateArgs args)
		{
			return Record.Lh >= 0;
		}

		protected virtual bool ValidatePc(IField field, IValidateArgs args)
		{
			return Record.Pc >= 0;
		}

		protected virtual bool ValidateSh01(IField field, IValidateArgs args)
		{
			// TODO: apply proper validation range

			return true;
		}

		protected virtual bool ValidateLm(IField field, IValidateArgs args)
		{
			return Record.Lm >= 0 && Record.Lm <= 1;
		}

		protected virtual bool ValidateEt(IField field, IValidateArgs args)
		{
			return Record.Et >= 0 && Record.Et <= 1;
		}

		protected virtual bool ValidateBk(IField field, IValidateArgs args)
		{
			return Record.Bk >= 0;
		}

		protected override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				var fields = base.GetFields();

				fields.AddRange(new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Fl";
						x.Validate = ValidateFl;
						x.GetValue = () => Record.Fl;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Lu";
						x.Validate = ValidateLu;
						x.GetValue = () => Record.Lu;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Mc";
						x.Validate = ValidateMc;
						x.GetValue = () => Record.Mc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Mf";
						x.Validate = ValidateMf;
						x.GetValue = () => Record.Mf;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Ma";
						x.Validate = ValidateMa;
						x.GetValue = () => Record.Ma;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Mz";
						x.Validate = ValidateMz;
						x.GetValue = () => Record.Mz;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Tp";
						x.Validate = ValidateTp;
						x.GetValue = () => Record.Tp;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "By";
						x.Validate = ValidateBy;
						x.GetValue = () => Record.By;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Pr";
						x.Validate = ValidatePr;
						x.GetValue = () => Record.Pr;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Bt";
						x.Validate = ValidateBt;
						x.GetValue = () => Record.Bt;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Tk";
						x.Validate = ValidateTk;
						x.GetValue = () => Record.Tk;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Cw";
						x.Validate = ValidateCw;
						x.GetValue = () => Record.Cw;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Lh";
						x.Validate = ValidateLh;
						x.GetValue = () => Record.Lh;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Pc";
						x.Validate = ValidatePc;
						x.GetValue = () => Record.Pc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Sh01";
						x.Validate = ValidateSh01;
						x.GetValue = () => Record.Sh01;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Lm";
						x.Validate = ValidateLm;
						x.GetValue = () => Record.Lm;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Et";
						x.Validate = ValidateEt;
						x.GetValue = () => Record.Et;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Bk";
						x.Validate = ValidateBk;
						x.GetValue = () => Record.Bk;
					}),
				});
			}

			return Fields;
		}
	}
}
