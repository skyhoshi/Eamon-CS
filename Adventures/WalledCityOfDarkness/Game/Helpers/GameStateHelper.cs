
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;

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
	}
}
