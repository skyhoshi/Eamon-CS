
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual bool EfreetiKilled { get; set; }

		public virtual bool WaterWeirdKilled { get; set; }

		public virtual bool AmoebaAppeared { get; set; }

		public virtual bool ExitDirNames { get; set; }

		public virtual long PlayerResurrections { get; set; }

		public virtual long EfreetiSummons { get; set; }

		public virtual long TorchRounds { get; set; }
	}
}
