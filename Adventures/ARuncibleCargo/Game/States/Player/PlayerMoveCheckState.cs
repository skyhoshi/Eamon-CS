
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void PrintRideOffIntoSunset()
		{
			Globals.Out.Print("You ride off into the moonlight.");
		}

		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterBlockingArtifactCheck && Globals.GameState.R2 == -34)
			{
				Globals.Out.Print("The broken opening is too small to fit through.");
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
