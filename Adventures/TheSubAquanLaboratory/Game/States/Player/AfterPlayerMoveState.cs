
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		protected override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeAfterExtinguishLightSourceCheck && gameState.R3 == 43 && gameState.Ro != gameState.R3)
			{
				var glassWallsArtifact = Globals.ADB[84];

				Debug.Assert(glassWallsArtifact != null);

				if (!glassWallsArtifact.IsInLimbo())
				{
					var ovalDoorArtifact = Globals.ADB[16];

					Debug.Assert(ovalDoorArtifact != null);

					if (ovalDoorArtifact.IsInLimbo())
					{
						ovalDoorArtifact.SetInRoomUid(43);
					}
				}

				gameState.Sterilize = false;
			}

			base.ProcessEvents(eventType);
		}
	}
}
