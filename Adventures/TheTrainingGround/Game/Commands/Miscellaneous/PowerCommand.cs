
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				var hammerArtifact = gADB[24];

				Debug.Assert(hammerArtifact != null);

				// Thor's hammer appears in Norse Mural room

				if (gActorRoom.Uid == 22 && !gGameState.ThorsHammerAppears)
				{
					hammerArtifact.SetInRoom(gActorRoom);

					gEngine.PrintEffectDesc(7);

					gGameState.ThorsHammerAppears = true;

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				var rl = gEngine.RollDice(1, 100, 0);

				// 20% chance of gender change

				if (rl < 21 && gGameState.GenderChangeCounter < 2)
				{
					gActorMonster.Gender = gActorMonster.EvalGender(Gender.Female, Gender.Male, Gender.Neutral);

					gCharacter.Gender = gActorMonster.Gender;

					gOut.Print("You feel different... more {0}.", gActorMonster.EvalGender("masculine", "feminine", "androgynous"));

					gGameState.GenderChangeCounter++;

					GotoCleanup = true;

					goto Cleanup;
				}

				// 40% chance Charisma up (one time only)

				if (rl < 41 && !gGameState.CharismaBoosted)
				{
					gCharacter.ModStats(Stat.Charisma, 2);

					gOut.Print("You suddenly feel more {0}.", gCharacter.EvalGender("handsome", "beautiful", "androgynous"));

					gGameState.CharismaBoosted = true;

					GotoCleanup = true;

					goto Cleanup;
				}

				// 5% Chance of being hit by lightning!

				if (rl > 94)
				{
					gEngine.PrintEffectDesc(33);

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = gActorMonster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 10);

					GotoCleanup = true;

					goto Cleanup;
				}

				PrintSonicBoom();
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}

		Cleanup:

			;
		}
	}
}
