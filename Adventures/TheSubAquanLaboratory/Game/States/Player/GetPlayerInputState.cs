
// GetPlayerInputState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IGetPlayerInputState))]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		protected override void ProcessEvents()
		{
			Eamon.Framework.IArtifact artifact;

			if (ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as IGameState;

				Debug.Assert(gameState != null);

				// Electrified floor

				artifact = Globals.ADB[85];

				Debug.Assert(artifact != null);

				if (artifact.Location == Globals.GameState.Ro)
				{
					Globals.Out.Write("{0}The electrified floor zaps everyone in the chamber!{0}", Environment.NewLine);

					var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsCharacterMonster(), m => m.Location == Globals.GameState.Ro && !m.IsCharacterMonster());

					for (var i = 0; i < monsters.Count; i++)
					{
						var monster = monsters[i];

						var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
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
							artifact = Globals.ADB[52];

							Debug.Assert(artifact != null);

							if (artifact.IsWornByCharacter())
							{
								Globals.Out.Write("{0}The chamber has entirely flooded!{0}", Environment.NewLine);

								var monsters = Globals.Engine.GetMonsterList(() => true, m => m.Location == gameState.Ro && !m.IsCharacterMonster());

								for (var i = 0; i < monsters.Count; i++)
								{
									var monster = monsters[i];

									monster.DmgTaken = monster.Hardiness;

									var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
									{
										x.SetNextStateFunc = s => NextState = s;

										x.DfMonster = monster;

										x.OmitFinalNewLine = true /* i == monsters.Count - 1 */ ? false : true;
									});

									combatSystem.ExecuteCheckMonsterStatus();
								}

								Globals.RtEngine.CheckEnemies();
							}
							else
							{
								Globals.Engine.PrintEffectDesc(33);

								gameState.Die = 1;

								NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
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
								Globals.Out.Write("{0}The water has risen to the {1} meter mark.{0}", Environment.NewLine, gameState.FloodLevel / 3);
							}
							else
							{
								Globals.Out.Write("{0}The water continues to pour into the chamber.{0}", Environment.NewLine);
							}
						}
					}
					else if (gameState.Flood == 2)
					{
						if (gameState.FloodLevel % 3 == 0)
						{
							Globals.Out.Write("{0}The water has receded to the {1} meter mark.{0}", Environment.NewLine, gameState.FloodLevel / 3);
						}
						else
						{
							Globals.Out.Write("{0}The water continues to drain from the chamber.{0}", Environment.NewLine);
						}

						if (--gameState.FloodLevel < 0)
						{
							gameState.Flood = 0;

							gameState.FloodLevel = 0;
						}
					}
				}
			}

			base.ProcessEvents();

		Cleanup:

			;
		}
	}
}
