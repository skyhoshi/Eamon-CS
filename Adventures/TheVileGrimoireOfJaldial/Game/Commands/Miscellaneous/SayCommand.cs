﻿
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
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
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				var efreetiMonster = gMDB[50];

				Debug.Assert(efreetiMonster != null);

				var parchmentArtifact = gADB[33];

				Debug.Assert(parchmentArtifact != null);

				// Summon efreeti

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

				// Kill water weird

				else if (waterWeirdMonster.IsInRoom(gActorRoom) && string.Equals(ProcessedPhrase, "avarchrom yarei uttoximo", StringComparison.OrdinalIgnoreCase))
				{
					gOut.Print("{0} jolts violently several times and then disintegrates.", waterWeirdMonster.GetTheName(true));

					waterWeirdMonster.DmgTaken = waterWeirdMonster.Hardiness;

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = waterWeirdMonster;

						// x.OmitFinalNewLine = false;
					});

					combatSystem.ExecuteCheckMonsterStatus();
				}
			}

			base.PlayerProcessEvents(eventType);
		}
	}
}
