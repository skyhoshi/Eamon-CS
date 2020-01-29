
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSay)
			{
				var efreetiMonster = gMDB[50];

				Debug.Assert(efreetiMonster != null);

				var parchmentArtifact = gADB[33];

				Debug.Assert(parchmentArtifact != null);

				if ((parchmentArtifact.IsCarriedByCharacter() || parchmentArtifact.IsInRoom(gActorRoom)) && efreetiMonster.IsInLimbo() && string.Equals(ProcessedPhrase, "rinnuk aukasker frudasdus", StringComparison.OrdinalIgnoreCase))
				{
					if (!gGameState.EfreetiKilled && ++gGameState.EfreetiSummons <= 3)
					{
						gOut.Print("You quote the words and hear a loud explosion, as a large infernal being appears in a whirlwind of fire!");

						efreetiMonster.SetInRoom(gActorRoom);
					}
					else
					{
						gOut.Print("You quote the words, and the parchment suddenly crumbles to dust!");

						parchmentArtifact.SetInLimbo();
					}
				}
			}

			base.PlayerProcessEvents(eventType);
		}
	}
}
