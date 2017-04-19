
// ExamineCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IExamineCommand))]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		protected override void PlayerProcessEvents()
		{
			var artifact = Globals.ADB[2];

			Debug.Assert(artifact != null);

			var artifact01 = Globals.ADB[4];

			Debug.Assert(artifact01 != null);

			// Armoire (while wearing glasses)

			if (DobjArtifact.Uid == 3 && artifact.IsWornByCharacter() && !artifact01.IsInRoom(ActorRoom))
			{
				var ac = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Container);

				Debug.Assert(ac != null);

				ac.SetOpen(false);

				var command = Globals.CreateInstance<EamonRT.Framework.Commands.IOpenCommand>();

				CopyCommandData(command);

				NextState = command;

				GotoCleanup = true;
			}

			// Bookshelf/secret door in library (while wearing magic glasses)

			else if (DobjArtifact.Uid == 11 && artifact.IsWornByCharacter())
			{
				artifact01 = Globals.ADB[10];

				Debug.Assert(artifact01 != null);

				var ac = artifact01.GetArtifactClass(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				artifact01.SetInRoom(ActorRoom);

				ac.SetOpen(true);

				ac.Field8 = 0;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}
	}
}
