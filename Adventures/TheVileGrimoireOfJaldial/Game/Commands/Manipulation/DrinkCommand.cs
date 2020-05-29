
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// The Molten Iron Ale is a critical resource in the final battle with Jaldi'al... but it also restores Hardiness drained by shadows.  What to do?

			if (eventType == PpeAfterArtifactDrink && gDobjArtifact.Uid == 15 && gGameState.PlayerHardinessPointsDrained > 0)
			{
				gOut.Print("You suddenly feel stronger as the warm afterglow of the ale fades!");

				gActorMonster.Hardiness++;

				gGameState.PlayerHardinessPointsDrained--;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
