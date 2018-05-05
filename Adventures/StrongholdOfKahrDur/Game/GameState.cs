
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings(typeof(Eamon.Framework.IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual bool UsedCauldron { get; set; }

		public virtual long LichState { get; set; }
	}
}
