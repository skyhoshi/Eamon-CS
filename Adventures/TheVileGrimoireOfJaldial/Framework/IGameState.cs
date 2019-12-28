
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		bool ExitDirNames { get; set; }
	}
}