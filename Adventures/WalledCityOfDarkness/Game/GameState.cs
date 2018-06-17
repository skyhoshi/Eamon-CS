
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace WalledCityOfDarkness.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long Fl { get; set; }

		public virtual long Lu { get; set; }

		public virtual long Mc { get; set; }

		public virtual long Mf { get; set; }

		public virtual long Ma { get; set; }

		public virtual long Mz { get; set; }

		public virtual long Tp { get; set; }

		public virtual long By { get; set; }

		public virtual long Pr { get; set; }

		public virtual long Bt { get; set; }

		public virtual long Tk { get; set; }

		public virtual long Cw { get; set; }

		public virtual long Lh { get; set; }

		public virtual long Pc { get; set; }

		public virtual long Sh01 { get; set; }

		public virtual long Lm { get; set; }

		public virtual long Et { get; set; }

		public virtual long Bk { get; set; }

		public GameState()
		{
			// TODO: initialize properties
		}
	}
}
