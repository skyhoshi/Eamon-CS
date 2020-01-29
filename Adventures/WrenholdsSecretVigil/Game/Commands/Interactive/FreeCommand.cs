
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var deviceArtifact = gADB[44];

			Debug.Assert(deviceArtifact != null);

			// Free caged animals

			if (eventType == PpeBeforeGuardMonsterCheck && gDobjArtifact.Uid == 46 && (deviceArtifact.IsInRoom(gActorRoom) || deviceArtifact.IsEmbeddedInRoom(gActorRoom)))
			{
				gOut.Print("The glowing cages won't open!");

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
