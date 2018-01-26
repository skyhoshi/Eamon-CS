
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using BeginnersForest.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IWearCommand))]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		protected override void PlayerProcessEvents()
		{
			// Magic Excercise Ring

			if (DobjArtifact.Uid == 2 && Globals.GameState.Speed <= 0)
			{
				var command = Globals.CreateInstance<EamonRT.Framework.Commands.ISpeedCommand>(x =>
				{
					x.CastSpell = false;
				});

				CopyCommandData(command);

				NextState = command;

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}
	}
}
