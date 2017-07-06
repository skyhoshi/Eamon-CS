
// IGameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace BeginnersForest.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		long QueenGiftEffectUid { get; set; }

		long QueenGiftArtifactUid { get; set; }

		long SpookCounter { get; set; }

		#endregion
	}
}
