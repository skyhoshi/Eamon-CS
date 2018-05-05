
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, EamonRT.Framework.States.IEndOfRoundState
	{
		protected override void ProcessEvents()
		{
			// Try to open running device, all flee

			if (Globals.DeviceOpened)
			{
				Globals.Out.Print("Your attempts to open the glowing device are unsuccessful.");

				Globals.DeviceOpened = false;
			}

			base.ProcessEvents();
		}
	}
}

