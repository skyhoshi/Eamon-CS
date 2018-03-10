
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IOpenCommand))]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		protected override void PlayerProcessEvents01()
		{
			var artifact = Globals.ADB[2];

			Debug.Assert(artifact != null);

			// If armoire opened and player is wearing eyeglasses reveal secret door

			if (DobjArtifact.Uid == 3 && artifact.IsWornByCharacter())
			{
				var artifact01 = Globals.ADB[4];

				Debug.Assert(artifact01 != null);

				var ac = artifact01.GetArtifactCategory(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				artifact01.SetInRoom(ActorRoom);

				ac.SetOpen(true);

				ac.Field4 = 0;
			}
			else
			{
				base.PlayerProcessEvents01();
			}
		}
	}
}
