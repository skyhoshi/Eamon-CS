
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace BeginnersForest.Game.Helpers
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

		protected virtual bool ValidateQueenGiftEffectUid()
		{
			return Record.QueenGiftEffectUid >= 5 && Record.QueenGiftEffectUid <= 6;
		}

		protected virtual bool ValidateQueenGiftArtifactUid()
		{
			return Record.QueenGiftArtifactUid == 7 || Record.QueenGiftArtifactUid == 15;
		}

		protected virtual bool ValidateSpookCounter()
		{
			return Record.SpookCounter >= 0 && Record.SpookCounter <= 10;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"QueenGiftEffectUid",
				"QueenGiftArtifactUid",
				"SpookCounter",
			});
		}
	}
}
