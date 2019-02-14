
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace BeginnersForest.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long QueenGiftEffectUid { get; set; }

		/// <summary></summary>
		long QueenGiftArtifactUid { get; set; }

		/// <summary></summary>
		long SpookCounter { get; set; }

		#endregion
	}
}
