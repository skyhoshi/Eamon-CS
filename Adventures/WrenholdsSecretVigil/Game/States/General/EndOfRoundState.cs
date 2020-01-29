
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(long eventType)
		{
			// Try to open running device, all flee

			if (eventType == PeAfterRoundEnd && Globals.DeviceOpened)
			{
				gOut.Print("Your attempts to open the glowing device are unsuccessful.");

				Globals.DeviceOpened = false;
			}

			base.ProcessEvents(eventType);
		}
	}
}

