
// OpenCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IOpenCommand))]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		protected override void PlayerProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			// Large cabinet

			if (DobjArtifact.Uid == 11 && !gameState.CabinetOpen)
			{
				Globals.Engine.PrintEffectDesc(34);

				gameState.CabinetOpen = true;
			}

			// Locker

			if (DobjArtifact.Uid == 51 && !gameState.LockerOpen)
			{
				Globals.Engine.PrintEffectDesc(36);

				gameState.LockerOpen = true;
			}

			base.PlayerProcessEvents();
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Humming cabinet

			if (DobjArtifact.Uid == 49)
			{
				Globals.Engine.PrintEffectDesc(35);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
