
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeCommandPromptPrint && ShouldPreTurnProcess())
			{
				// Electrified floor

				var electrifiedFloorArtifact = gADB[85];

				Debug.Assert(electrifiedFloorArtifact != null);

				if (electrifiedFloorArtifact.Location == gGameState.Ro)
				{
					gOut.Print("The electrified floor zaps everyone in the chamber!");

					var monsters = gEngine.GetMonsterList(m => m.IsCharacterMonster(), m => m.Location == gGameState.Ro && !m.IsCharacterMonster());

					for (var i = 0; i < monsters.Count; i++)
					{
						var monster = monsters[i];

						var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.DfMonster = monster;

							x.OmitArmor = true;

							x.OmitFinalNewLine = true /* monster.IsCharacterMonster() || i == monsters.Count - 1 */ ? false : true;
						});

						combatSystem.ExecuteCalculateDamage(1, 4);

						if (gGameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Flood water

				if (gGameState.Flood > 0 && gGameState.Ro == 10)
				{
					if (gGameState.Flood == 1)
					{
						if (gGameState.FloodLevel == 11)
						{
							var scubaGearArtifact = gADB[52];

							Debug.Assert(scubaGearArtifact != null);

							if (scubaGearArtifact.IsWornByCharacter())
							{
								gOut.Print("The chamber has entirely flooded!");

								var monsters = gEngine.GetMonsterList(m => m.Location == gGameState.Ro && !m.IsCharacterMonster());

								for (var i = 0; i < monsters.Count; i++)
								{
									var monster = monsters[i];

									monster.DmgTaken = monster.Hardiness;

									var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
									{
										x.SetNextStateFunc = s => NextState = s;

										x.DfMonster = monster;

										x.OmitFinalNewLine = true /* i == monsters.Count - 1 */ ? false : true;
									});

									combatSystem.ExecuteCheckMonsterStatus();
								}
							}
							else
							{
								gEngine.PrintEffectDesc(33);

								gGameState.Die = 1;

								NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
								{
									x.PrintLineSep = true;
								});

								GotoCleanup = true;

								goto Cleanup;
							}
						}
						else
						{
							if (++gGameState.FloodLevel % 3 == 0)
							{
								gOut.Print("The water has risen to the {0} meter mark.", gGameState.FloodLevel / 3);
							}
							else
							{
								gOut.Print("The water continues to pour into the chamber.");
							}
						}
					}
					else if (gGameState.Flood == 2)
					{
						if (gGameState.FloodLevel % 3 == 0)
						{
							gOut.Print("The water has receded to the {0} meter mark.", gGameState.FloodLevel / 3);
						}
						else
						{
							gOut.Print("The water continues to drain from the chamber.");
						}

						if (--gGameState.FloodLevel < 0)
						{
							gGameState.Flood = 0;

							gGameState.FloodLevel = 0;
						}
					}
				}
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
