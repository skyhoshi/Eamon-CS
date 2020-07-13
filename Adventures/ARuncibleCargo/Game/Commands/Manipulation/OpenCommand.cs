﻿
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Skip 'item opened' msg for Cargo

			if (artifact.Uid != 129)
			{
				base.PrintOpened(artifact);
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.Uid == 129 ? DobjArtifact.InContainer : null;

			// Open the Runcible Cargo

			if (DobjArtifact.Uid == 129 && !ac.IsOpen())
			{
				gEngine.PrintEffectDesc(129);

				gGameState.CargoOpenCounter = 0;
			}

			base.PlayerExecute();
		}
	}
}
