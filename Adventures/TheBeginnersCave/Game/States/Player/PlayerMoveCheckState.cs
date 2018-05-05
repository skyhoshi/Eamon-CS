
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, EamonRT.Framework.States.IPlayerMoveCheckState
	{
		protected override void ProcessEvents01()
		{
			if (Globals.GameState.R2 == -1)
			{
				Globals.Out.Print("Sorry, but I'm afraid to go into the water without my life preserver.");

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
