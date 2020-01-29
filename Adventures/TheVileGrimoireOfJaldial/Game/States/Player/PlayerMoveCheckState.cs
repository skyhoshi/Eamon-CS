
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterBlockingArtifactCheck && gGameState.R2 == -17)
			{
				gOut.Print("To go in that direction would mean certain death.");
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
