
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, EamonRT.Framework.Commands.IFreeCommand
	{
		protected override void PlayerProcessEvents()
		{
			var deviceArtifact = Globals.ADB[44];

			Debug.Assert(deviceArtifact != null);

			// Free caged animals

			if (DobjArtifact.Uid == 46 && (deviceArtifact.IsInRoom(ActorRoom) || deviceArtifact.IsEmbeddedInRoom(ActorRoom)))
			{
				Globals.Out.Print("The glowing cages won't open!");

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}
	}
}
