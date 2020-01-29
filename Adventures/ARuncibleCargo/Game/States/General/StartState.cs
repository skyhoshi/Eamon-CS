
// StartState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class StartState : EamonRT.Game.States.StartState, IStartState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeRoundStart)
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				var cargoArtifact = gADB[129];

				Debug.Assert(cargoArtifact != null);

				// Cargo check

				gGameState.CargoInRoom = cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByCharacter() ? 1 : 0;
			}

			base.ProcessEvents(eventType);
		}
	}
}
