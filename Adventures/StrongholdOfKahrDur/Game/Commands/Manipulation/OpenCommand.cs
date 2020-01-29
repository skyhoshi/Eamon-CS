
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var eyeglassesArtifact = gADB[2];

			Debug.Assert(eyeglassesArtifact != null);

			// If armoire opened and player is wearing eyeglasses reveal secret door

			if (eventType == PpeAfterArtifactOpen && gDobjArtifact.Uid == 3 && eyeglassesArtifact.IsWornByCharacter())
			{
				var secretDoorArtifact = gADB[4];

				Debug.Assert(secretDoorArtifact != null);

				var ac = secretDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				secretDoorArtifact.SetInRoom(gActorRoom);

				ac.SetOpen(true);

				ac.Field4 = 0;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
