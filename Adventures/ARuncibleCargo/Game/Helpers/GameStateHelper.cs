
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace ARuncibleCargo.Game.Helpers
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

		protected virtual bool ValidateDreamCounter()
		{
			return Record.DreamCounter >= 0 && Record.DreamCounter <= 13;
		}

		protected virtual bool ValidateSwarmyCounter()
		{
			return Record.SwarmyCounter >= 1 && Record.SwarmyCounter <= 3;
		}

		protected virtual bool ValidateCargoOpenCounter()
		{
			return Record.CargoOpenCounter >= 0 && Record.CargoOpenCounter <= 3;
		}

		protected virtual bool ValidateCargoInRoom()
		{
			return Record.CargoInRoom >= 0 && Record.CargoInRoom <= 1;
		}

		protected virtual bool ValidateGiveAmazonMoney()
		{
			return Record.GiveAmazonMoney >= 0 && Record.GiveAmazonMoney <= 1;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"DreamCounter",
				"SwarmyCounter",
				"CargoOpenCounter",
				"CargoInRoom",
				"GiveAmazonMoney",
			});
		}
	}
}
