
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		/// <summary></summary>
		bool EfreetiKilled { get; set; }

		/// <summary></summary>
		bool WaterWeirdKilled { get; set; }

		/// <summary></summary>
		bool AmoebaAppeared { get; set; }

		/// <summary></summary>
		bool ExitDirNames { get; set; }

		/// <summary></summary>
		long PlayerResurrections { get; set; }

		/// <summary></summary>
		long EfreetiSummons { get; set; }

		/// <summary></summary>
		long TorchRounds { get; set; }
	}
}
