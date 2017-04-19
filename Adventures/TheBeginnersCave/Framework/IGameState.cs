
// IGameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace TheBeginnersCave.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		long Trollsfire { get; set; }

		long BookWarning { get; set; }

		long UsedWpnIdx { get; set; }

		long[] HeldWpnUids { get; set; }

		#endregion

		#region Methods

		long GetHeldWpnUids(long index);

		void SetHeldWpnUids(long index, long value);

		#endregion
	}
}
