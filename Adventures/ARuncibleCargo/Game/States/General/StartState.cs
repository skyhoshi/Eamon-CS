
// StartState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class StartState : EamonRT.Game.States.StartState, IStartState
	{
		protected override void ProcessEvents()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			var room = Globals.RDB[gameState.Ro];

			Debug.Assert(room != null);

			var artifact = Globals.ADB[129];

			Debug.Assert(artifact != null);

			// Cargo check

			gameState.CargoInRoom = artifact.IsInRoom(room) || artifact.IsCarriedByCharacter() ? 1 : 0;

			base.ProcessEvents();
		}
	}
}
