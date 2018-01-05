
// EndOfRoundState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IEndOfRoundState))]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		protected override void ProcessEvents()
		{
			// Try to open running device, all flee

			if (Globals.DeviceOpened)
			{
				Globals.Out.Write("{0}Your attempts to open the glowing device are unsuccessful.{0}", Environment.NewLine);

				Globals.DeviceOpened = false;
			}

			base.ProcessEvents();
		}
	}
}

