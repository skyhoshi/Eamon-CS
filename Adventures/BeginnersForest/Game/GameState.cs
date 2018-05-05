
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(Eamon.Framework.IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long QueenGiftEffectUid { get; set; }

		public virtual long QueenGiftArtifactUid { get; set; }

		public virtual long SpookCounter { get; set; }

		public GameState()
		{
			// Queen's gift

			QueenGiftEffectUid = 5;

			QueenGiftArtifactUid = 7;
		}
	}
}
