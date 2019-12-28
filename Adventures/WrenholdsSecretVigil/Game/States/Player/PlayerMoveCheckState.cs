
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(long eventType)
		{
			RetCode rc;

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeBeforeCanMoveToRoomCheck)
			{
				var room5 = Globals.RDB[5];

				Debug.Assert(room5 != null);

				var room40 = Globals.RDB[40];

				Debug.Assert(room40 != null);

				var room45 = Globals.RDB[45];

				Debug.Assert(room45 != null);

				// Auto-reveal secret doors if necessary

				if (gameState.R2 == 5 && gameState.Ro == 8 && room5.GetDirs(Direction.South) == -8)
				{
					room5.SetDirs(Direction.South, 8);
				}
				else if (gameState.R2 == 40 && gameState.Ro == 41 && room40.GetDirs(Direction.South) == -41)
				{
					room40.SetDirs(Direction.South, 41);
				}
				else if (gameState.R2 == 45 && gameState.Ro == 43 && room45.GetDirs(Direction.East) == -43)
				{
					room45.SetDirs(Direction.East, 43);
				}

				// Falling down a drop-off (injury)

				if (gameState.R2 == 25 && (gameState.Ro == 24 || gameState.Ro == 27) && Direction != Direction.Down)
				{
					Globals.Engine.PrintEffectDesc(24);

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = Monster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 1);

					if (gameState.Die > 0)
					{
						GotoCleanup = true;
					}
				}

				// Falling down a drop-off (no injury)

				else if (gameState.R2 == 18 && gameState.Ro == 17 && Direction != Direction.Down)
				{
					Globals.Engine.PrintEffectDesc(5);
				}
				else
				{
					base.ProcessEvents(eventType);
				}
			}
			else if (eventType == PeAfterBlockingArtifactCheck)
			{
				var ropeArtifact = Globals.ADB[28];

				Debug.Assert(ropeArtifact != null);

				// Down the airshaft

				if (gameState.R2 == -100)
				{
					Globals.Engine.PrintEffectDesc(42);

					gameState.R2 = 36;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>();
				}

				// Over the cliff

				else if (gameState.R2 == -101)
				{
					Globals.Engine.PrintEffectDesc(1);

					gameState.Die = 1;

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}

				// Up rope rings bell

				else if (gameState.R2 == -102)
				{
					Globals.Engine.RevealEmbeddedArtifact(Room, ropeArtifact);

					Globals.Engine.PrintEffectDesc(25);

					gameState.R2 = gameState.Ro;

					if (!gameState.PulledRope)
					{
						var monsters = Globals.Engine.GetMonsterList(m => m.Uid >= 14 && m.Uid <= 16);

						foreach (var monster in monsters)
						{
							monster.SetInRoomUid(48);
						}

						gameState.PulledRope = true;
					}

					PrintCantGoThatWay();
				}
				else if (gameState.R2 == Constants.DirectionExit)
				{
					Globals.Out.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						var lifeOrbArtifact = Globals.ADB[4];

						Debug.Assert(lifeOrbArtifact != null);

						var carryingLifeOrb = lifeOrbArtifact.IsCarriedByCharacter();

						var lifeOrbInPedestal = lifeOrbArtifact.IsCarriedByContainerUid(43);

						lifeOrbArtifact.SetInLimbo();

						Globals.Out.Print("{0}", Globals.LineSep);

						// If life-orb carried by player character or in metal pedestal

						if (carryingLifeOrb || lifeOrbInPedestal)
						{
							if (carryingLifeOrb)
							{
								Globals.Engine.PrintEffectDesc(31);
							}
							else
							{
								Globals.Engine.PrintEffectDesc(35);

								Globals.Engine.PrintEffectDesc(36);
							}

							// Name magic elven bow

							var magicBowArtifact = Globals.ADB[50];

							Debug.Assert(magicBowArtifact != null);

							magicBowArtifact.SetCarriedByCharacter();

							Globals.Out.Print("King Argas hands you the bow and one arrow.  You nock then let loose the arrow...");

							Globals.Out.Write("{0}What will you name your bow? ", Environment.NewLine);

							Globals.Buf.Clear();

							rc = Globals.In.ReadField(Globals.Buf, Constants.CharArtNameLen, null, ' ', '\0', false, null, null, null, null);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString(), @"\s+", " ").Trim());

							var wpnName = Globals.Buf.ToString();

							if (wpnName == "" || string.Equals(wpnName, "NONE", StringComparison.OrdinalIgnoreCase))
							{
								wpnName = "whisperwind";
							}

							magicBowArtifact.Name = Globals.Engine.Capitalize(wpnName.ToLower());

							if (carryingLifeOrb)
							{
								Globals.Engine.PrintEffectDesc(32);
							}
							else
							{
								Globals.Engine.PrintEffectDesc(37);
							}

							Globals.Character.HeldGold += 2000;

							gameState.Die = 0;

							Globals.ExitType = ExitType.FinishAdventure;

							Globals.MainLoop.ShouldShutdown = true;
						}
						else     // The rabid animals attack
						{
							for (var i = 38; i <= 41; i++)
							{
								Globals.Engine.PrintEffectDesc(i);
							}

							Globals.Out.Print("You are dead.");

							gameState.Die = 1;

							NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							});
						}
					}
				}
				else
				{
					base.ProcessEvents(eventType);
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
