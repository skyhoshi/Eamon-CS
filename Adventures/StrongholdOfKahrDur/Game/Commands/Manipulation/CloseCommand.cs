﻿
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// If the armoire is closed then hide the secret passage

			if (eventType == PpeAfterArtifactClose && DobjArtifact.Uid == 3)
			{
				var secretDoorArtifact = gADB[4];

				Debug.Assert(secretDoorArtifact != null);

				var ac = secretDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				secretDoorArtifact.SetInLimbo();

				ac.SetOpen(false);

				ac.Field4 = 1;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
