
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
				var gameState = Globals.GameState as Framework.IGameState;

				Debug.Assert(gameState != null);

				// Electrified floor

				var electrifiedFloorArtifact = Globals.ADB[85];

				Debug.Assert(electrifiedFloorArtifact != null);

				if (electrifiedFloorArtifact.Location == Globals.GameState.Ro)
				{
					Globals.Out.Print("The electrified floor zaps everyone in the chamber!");

					var monsters = Globals.Engine.GetMonsterList(m => m.IsCharacterMonster(), m => m.Location == Globals.GameState.Ro && !m.IsCharacterMonster());

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

						if (Globals.GameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Flood water

				if (gameState.Flood > 0 && gameState.Ro == 10)
				{
					if (gameState.Flood == 1)
					{
						if (gameState.FloodLevel == 11)
						{
							var scubaGearArtifact = Globals.ADB[52];

							Debug.Assert(scubaGearArtifact != null);

							if (scubaGearArtifact.IsWornByCharacter())
							{
								Globals.Out.Print("The chamber has entirely flooded!");

								var monsters = Globals.Engine.GetMonsterList(m => m.Location == gameState.Ro && !m.IsCharacterMonster());

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

								Globals.Engine.CheckEnemies();
							}
							else
							{
								Globals.Engine.PrintEffectDesc(33);

								gameState.Die = 1;

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
							if (++gameState.FloodLevel % 3 == 0)
							{
								Globals.Out.Print("The water has risen to the {0} meter mark.", gameState.FloodLevel / 3);
							}
							else
							{
								Globals.Out.Print("The water continues to pour into the chamber.");
							}
						}
					}
					else if (gameState.Flood == 2)
					{
						if (gameState.FloodLevel % 3 == 0)
						{
							Globals.Out.Print("The water has receded to the {0} meter mark.", gameState.FloodLevel / 3);
						}
						else
						{
							Globals.Out.Print("The water continues to drain from the chamber.");
						}

						if (--gameState.FloodLevel < 0)
						{
							gameState.Flood = 0;

							gameState.FloodLevel = 0;
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
