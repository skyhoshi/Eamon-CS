
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace TheBeginnersCave.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long Trollsfire { get; set; }

		/// <summary></summary>
		long BookWarning { get; set; }

		#endregion
	}
}
