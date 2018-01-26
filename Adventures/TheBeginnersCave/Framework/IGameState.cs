
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace TheBeginnersCave.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		long Trollsfire { get; set; }

		long BookWarning { get; set; }

		#endregion
	}
}
