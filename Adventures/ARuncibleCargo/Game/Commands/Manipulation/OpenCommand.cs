
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, EamonRT.Framework.Commands.IOpenCommand
	{
		protected override void PrintOpened(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Skip 'item opened' msg for Cargo

			if (artifact.Uid != 129)
			{
				base.PrintOpened(artifact);
			}
		}

		protected override bool ShouldPrintContainerInventory()
		{
			// Skip Cargo contents if empty

			if (DobjArtifact.Uid == 129)
			{
				var artifactList = DobjArtifact.GetContainedList();

				return artifactList.Count > 0;
			}
			else
			{
				return base.ShouldPrintContainerInventory();
			}
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var ac = DobjArtifact.Uid == 129 ? DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Container) : null;

			// Open the Runcible Cargo

			if (DobjArtifact.Uid == 129 && !ac.IsOpen())
			{
				Globals.Engine.PrintEffectDesc(129);

				gameState.CargoOpenCounter = 0;
			}

			base.PlayerExecute();
		}
	}
}
