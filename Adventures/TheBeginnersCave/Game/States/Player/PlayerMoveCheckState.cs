
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterBlockingArtifactCheck && Globals.GameState.R2 == -1)
			{
				Globals.Out.Print("Sorry, but I'm afraid to go into the water without my life preserver.");

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
