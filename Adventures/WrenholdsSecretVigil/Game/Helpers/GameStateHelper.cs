
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace WrenholdsSecretVigil.Game.Helpers
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

		protected virtual bool ValidateMedallionCharges()
		{
			return Record.MedallionCharges >= 0 && Record.MedallionCharges <= 15;
		}

		protected virtual bool ValidateSlimeBlasts()
		{
			return Record.SlimeBlasts >= 0 && Record.SlimeBlasts <= 3;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"MedallionCharges",
				"SlimeBlasts",
			});
		}
	}
}
