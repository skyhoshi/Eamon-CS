
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ICloseCommand))]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		protected override void PlayerProcessEvents()
		{
			// If the armoire is closed then hide the secret passage

			if (DobjArtifact.Uid == 3)
			{
				var artifact = Globals.ADB[4];

				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactCategory(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				artifact.SetInLimbo();

				ac.SetOpen(false);

				ac.Field4 = 1;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}
	}
}
