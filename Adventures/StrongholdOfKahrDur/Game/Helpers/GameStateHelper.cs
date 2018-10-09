
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace StrongholdOfKahrDur.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper, IGameStateHelper
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

		protected virtual bool ValidateLichState()
		{
			return Record.LichState >= 0 && Record.LichState <= 2;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"LichState"
			});
		}
	}
}
