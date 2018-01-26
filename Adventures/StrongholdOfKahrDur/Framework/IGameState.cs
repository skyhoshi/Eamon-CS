
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace StrongholdOfKahrDur.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		bool UsedCauldron { get; set; }

		long LichState { get; set; }
	}
}
