
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheWayfarersInn.Game.Plugin.PluginContext;

namespace TheWayfarersInn.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{

	}
}
