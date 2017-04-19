
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPlayerMoveCheckState))]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void PrintRideOffIntoSunset()
		{
			Globals.Out.WriteLine("{0}You ride off into the moonlight.", Environment.NewLine);
		}

		protected override void ProcessEvents01()
		{
			if (Globals.GameState.R2 == -34)
			{
				Globals.Out.WriteLine("{0}The broken opening is too small to fit through.", Environment.NewLine);
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
