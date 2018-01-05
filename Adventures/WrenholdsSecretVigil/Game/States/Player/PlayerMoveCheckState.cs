
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPlayerMoveCheckState))]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected virtual Framework.IGameState GameState { get; set; }

		protected override void ProcessEvents()
		{
			var room5 = Globals.RDB[5];

			Debug.Assert(room5 != null);

			var room40 = Globals.RDB[40];

			Debug.Assert(room40 != null);

			var room45 = Globals.RDB[45];

			Debug.Assert(room45 != null);

			// Auto-reveal secret doors if necessary

			if (GameState.R2 == 5 && GameState.Ro == 8 && room5.GetDirs(Enums.Direction.South) == -8)
			{
				room5.SetDirs(Enums.Direction.South, 8);
			}
			else if (GameState.R2 == 40 && GameState.Ro == 41 && room40.GetDirs(Enums.Direction.South) == -41)
			{
				room40.SetDirs(Enums.Direction.South, 41);
			}
			else if (GameState.R2 == 45 && GameState.Ro == 43 && room45.GetDirs(Enums.Direction.East) == -43)
			{
				room45.SetDirs(Enums.Direction.East, 43);
			}
			
			// Falling down a drop-off (injury)

			if (GameState.R2 == 25 && (GameState.Ro == 24 || GameState.Ro == 27) && Direction != Enums.Direction.Down)
			{
				Globals.Engine.PrintEffectDesc(24);

				var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.DfMonster = Monster;

					x.OmitArmor = true;
				});

				combatSystem.ExecuteCalculateDamage(1, 1);

				if (GameState.Die > 0)
				{
					GotoCleanup = true;
				}
			}

			// Falling down a drop-off (no injury)

			else if (GameState.R2 == 18 && GameState.Ro == 17 && Direction != Enums.Direction.Down)
			{
				Globals.Engine.PrintEffectDesc(5);
			}
			else
			{
				base.ProcessEvents();
			}
		}

		protected override void ProcessEvents01()
		{
			RetCode rc;

			var ropeArtifact = Globals.ADB[28];

			Debug.Assert(ropeArtifact != null);

			// Down the airshaft

			if (GameState.R2 == -100)
			{
				Globals.Engine.PrintEffectDesc(42);

				GameState.R2 = 36;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();
			}

			// Over the cliff

			else if (GameState.R2 == -101)
			{
				Globals.Engine.PrintEffectDesc(1);

				GameState.Die = 1;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});
			}

			// Up rope rings bell

			else if (GameState.R2 == -102)
			{
				Globals.Engine.RevealEmbeddedArtifact(Room, ropeArtifact);

				Globals.Engine.PrintEffectDesc(25);

				GameState.R2 = GameState.Ro;

				if (!GameState.PulledRope)
				{
					var monsters = Globals.Engine.GetMonsterList(() => true, m => m.Uid >= 14 && m.Uid <= 16);

					foreach (var monster in monsters)
					{
						monster.SetInRoomUid(48);
					}

					// CheckEnemies intentionally omitted

					GameState.PulledRope = true;
				}

				PrintCantGoThatWay();
			}
			else if (GameState.R2 == Constants.DirectionExit)
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

					Globals.Engine.RemoveWeight(lifeOrbArtifact);

					lifeOrbArtifact.SetInLimbo();

					Globals.Out.Write("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

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

						GameState.Wt += magicBowArtifact.Weight;

						magicBowArtifact.SetCarriedByCharacter();

						Globals.Out.Write("{0}King Argas hands you the bow and one arrow.  You nock then let loose the arrow...{0}", Environment.NewLine);

						Globals.Out.Write("{0}What will you name your bow? ", Environment.NewLine);

						Globals.Buf.Clear();

						rc = Globals.In.ReadField(Globals.Buf, Constants.CharWpnNameLen, null, ' ', '\0', false, null, null, null, null);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						var wpnName = Globals.Buf.Trim().ToString();

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

						GameState.Die = 0;

						Globals.ExitType = Enums.ExitType.FinishAdventure;

						Globals.MainLoop.ShouldShutdown = true;
					}
					else     // The rabid animals attack
					{
						for (var i = 38; i <= 41; i++)
						{
							Globals.Engine.PrintEffectDesc(i);
						}

						Globals.Out.Write("{0}You are dead.{0}", Environment.NewLine);

						GameState.Die = 1;

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});
					}
				}
			}
			else
			{
				base.ProcessEvents01();
			}
		}

		public PlayerMoveCheckState()
		{
			GameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(GameState != null);
		}
	}
}
