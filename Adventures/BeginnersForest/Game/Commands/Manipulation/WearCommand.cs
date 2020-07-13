
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// Magic Excercise Ring

			if (eventType == PpeAfterArtifactWear && DobjArtifact.Uid == 2 && gGameState.Speed <= 0)
			{
				var command = Globals.CreateInstance<ISpeedCommand>(x =>
				{
					x.CastSpell = false;
				});

				CopyCommandData(command);

				NextState = command;

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
