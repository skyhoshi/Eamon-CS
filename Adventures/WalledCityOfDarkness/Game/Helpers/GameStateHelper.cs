
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

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

		protected virtual bool ValidateFl()
		{
			return Record.Fl >= 0 && Record.Fl <= 2;
		}

		protected virtual bool ValidateLu()
		{
			return Record.Lu >= 0 && Record.Lu <= 2;
		}

		protected virtual bool ValidateMc()
		{
			return Record.Mc >= 0;
		}

		protected virtual bool ValidateMf()
		{
			return Record.Mf >= 0 && Record.Mf <= 1;
		}

		protected virtual bool ValidateMa()
		{
			return Record.Ma >= 0;
		}

		protected virtual bool ValidateMz()
		{
			// TODO: apply proper validation range

			return true;
		}

		protected virtual bool ValidateTp()
		{
			return Record.Tp >= 0 && Record.Tp <= 10;
		}

		protected virtual bool ValidateBy()
		{
			return Record.By >= 0;
		}

		protected virtual bool ValidatePr()
		{
			return Record.Pr >= 0 && Record.Pr <= 1;
		}

		protected virtual bool ValidateBt()
		{
			return Record.Bt >= 0 && Record.Bt <= 2;
		}

		protected virtual bool ValidateTk()
		{
			return Record.Tk >= 0 && Record.Tk <= 3;
		}

		protected virtual bool ValidateCw()
		{
			return Record.Cw >= 0 && Record.Cw <= 1;
		}

		protected virtual bool ValidateLh()
		{
			return Record.Lh >= 0;
		}

		protected virtual bool ValidatePc()
		{
			return Record.Pc >= 0;
		}

		protected virtual bool ValidateSh01()
		{
			// TODO: apply proper validation range

			return true;
		}

		protected virtual bool ValidateLm()
		{
			return Record.Lm >= 0 && Record.Lm <= 1;
		}

		protected virtual bool ValidateEt()
		{
			return Record.Et >= 0 && Record.Et <= 1;
		}

		protected virtual bool ValidateBk()
		{
			return Record.Bk >= 0;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"Fl",
				"Lu",
				"Mc",
				"Mf",
				"Ma",
				"Mz",
				"Tp",
				"By",
				"Pr",
				"Bt",
				"Tk",
				"Cw",
				"Lh",
				"Pc",
				"Sh01",
				"Lm",
				"Et",
				"Bk",
			});
		}
	}
}
