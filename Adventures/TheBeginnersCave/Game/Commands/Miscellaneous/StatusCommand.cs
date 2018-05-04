
// StatusCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheBeginnersCave.Framework;
using TheBeginnersCave.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IStatusCommand))]
	public class StatusCommand : EamonRT.Game.Commands.StatusCommand, IStatusCommand
	{
		protected override void PlayerProcessEvents()
		{
			var trollsfireArtifact = Globals.ADB[10];

			Debug.Assert(trollsfireArtifact != null);

			if (trollsfireArtifact.IsCarriedByCharacter() && Globals.GameState.CastTo<IGameState>().Trollsfire == 1)
			{
				Globals.Out.Print("Trollsfire is alight!");
			}

			base.PlayerProcessEvents();
		}
	}
}
