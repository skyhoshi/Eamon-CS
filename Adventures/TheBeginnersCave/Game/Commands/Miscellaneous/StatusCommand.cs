﻿
// StatusCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : EamonRT.Game.Commands.StatusCommand, IStatusCommand
	{
		public override void PlayerProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterPlayerStatus)
			{
				var trollsfireArtifact = gADB[10];

				Debug.Assert(trollsfireArtifact != null);

				if (trollsfireArtifact.IsCarriedByCharacter() && gGameState.Trollsfire == 1)
				{
					gOut.Print("Trollsfire is alight!");
				}
			}

			base.PlayerProcessEvents(eventType);
		}
	}
}
