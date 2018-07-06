
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheBeginnersCave.Game.Helpers
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

		protected virtual bool ValidateTrollsfire()
		{
			return Record.Trollsfire >= 0 && Record.Trollsfire <= 1;
		}

		protected virtual bool ValidateBookWarning()
		{
			return Record.BookWarning >= 0 && Record.BookWarning <= 1;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"Trollsfire",
				"BookWarning",
			});
		}
	}
}
