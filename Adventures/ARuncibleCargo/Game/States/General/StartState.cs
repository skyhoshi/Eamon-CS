
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
		protected override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeBeforeRoundStart)
			{
				var room = Globals.RDB[gameState.Ro];

				Debug.Assert(room != null);

				var cargoArtifact = Globals.ADB[129];

				Debug.Assert(cargoArtifact != null);

				// Cargo check

				gameState.CargoInRoom = cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByCharacter() ? 1 : 0;
			}

			base.ProcessEvents(eventType);
		}
	}
}
