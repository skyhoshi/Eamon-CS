
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterExtinguishLightSourceCheck && gGameState.R3 == 43 && gGameState.Ro != gGameState.R3)
			{
				var glassWallsArtifact = gADB[84];

				Debug.Assert(glassWallsArtifact != null);

				if (!glassWallsArtifact.IsInLimbo())
				{
					var ovalDoorArtifact = gADB[16];

					Debug.Assert(ovalDoorArtifact != null);

					if (ovalDoorArtifact.IsInLimbo())
					{
						ovalDoorArtifact.SetInRoomUid(43);
					}
				}

				gGameState.Sterilize = false;
			}

			base.ProcessEvents(eventType);
		}
	}
}
