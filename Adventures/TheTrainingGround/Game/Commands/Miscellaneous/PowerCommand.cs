
// PowerCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework;
using TheTrainingGround.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPowerCommand))]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected override void PlayerProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var hammerArtifact = Globals.ADB[24];

			Debug.Assert(hammerArtifact != null);

			// Thor's hammer appears in Norse Mural room

			if (ActorRoom.Uid == 22 && !gameState.ThorsHammerAppears)
			{
				hammerArtifact.SetInRoom(ActorRoom);

				Globals.Engine.PrintEffectDesc(7);

				gameState.ThorsHammerAppears = true;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;

				goto Cleanup;
			}

			var rl = Globals.Engine.RollDice01(1, 100, 0);

			// 20% chance of gender change

			if (rl < 21 && gameState.GenderChangeCounter < 2)
			{
				ActorMonster.Gender = ActorMonster.EvalGender(Enums.Gender.Female, Enums.Gender.Male, Enums.Gender.Neutral);

				Globals.Character.Gender = ActorMonster.Gender;

				Globals.Out.WriteLine("{0}You feel different... more {1}.", Environment.NewLine, ActorMonster.EvalGender("masculine", "feminine", "androgynous"));

				gameState.GenderChangeCounter++;

				GotoCleanup = true;

				goto Cleanup;
			}

			// 40% chance Charisma up (one time only)

			if (rl < 41 && !gameState.CharismaBoosted)
			{
				Globals.Character.ModStats(Enums.Stat.Charisma, 2);

				Globals.Out.WriteLine("{0}You suddenly feel more {1}.", Environment.NewLine, Globals.Character.EvalGender("handsome", "beautiful", "androgynous"));

				gameState.CharismaBoosted = true;

				GotoCleanup = true;

				goto Cleanup;
			}

			// 5% Chance of being hit by lightning!

			if (rl > 94)
			{
				Globals.Engine.PrintEffectDesc(33);

				var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.DfMonster = ActorMonster;

					x.OmitArmor = true;
				});

				combatSystem.ExecuteCalculateDamage(1, 10);

				GotoCleanup = true;

				goto Cleanup;
			}

			PrintSonicBoom();

		Cleanup:

			;
		}
	}
}
