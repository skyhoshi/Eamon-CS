
// FreeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IFreeCommand))]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		protected override void PlayerProcessEvents()
		{
			var deviceArtifact = Globals.ADB[44];

			Debug.Assert(deviceArtifact != null);

			// Free caged animals

			if (DobjArtifact.Uid == 46 && (deviceArtifact.IsInRoom(ActorRoom) || deviceArtifact.IsEmbeddedInRoom(ActorRoom)))
			{
				Globals.Out.Write("{0}The glowing cages won't open!{0}", Environment.NewLine);

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}
	}
}
