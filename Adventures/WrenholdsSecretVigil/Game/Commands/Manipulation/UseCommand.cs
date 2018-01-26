
// UseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IUseCommand))]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var deviceArtifact = Globals.ADB[44];

			Debug.Assert(deviceArtifact != null);

			// Use lever

			if (DobjArtifact.Uid == 48 && deviceArtifact.IsInRoom(ActorRoom))
			{
				var command = Globals.CreateInstance<EamonRT.Framework.Commands.IGetCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
