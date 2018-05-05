
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, EamonRT.Framework.States.IPlayerMoveCheckState
	{
		protected override void PrintRideOffIntoSunset()
		{
			Globals.Out.Print("You ride off into the moonlight.");
		}

		protected override void ProcessEvents01()
		{
			if (Globals.GameState.R2 == -34)
			{
				Globals.Out.Print("The broken opening is too small to fit through.");
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
