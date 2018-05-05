
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, EamonRT.Framework.States.IPlayerMoveCheckState
	{
		protected override void PrintRideOffIntoSunset()
		{
			Globals.Out.Print("You ride off into the sunset.");
		}

		protected override void ProcessEvents()
		{
			RetCode rc;

			var monster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(monster != null);

			var artifact = Globals.ADB[18];

			Debug.Assert(artifact != null);

			// Cannot enter forest if not wearing magical amulet

			if (Globals.GameState.Ro == 92 && Globals.GameState.R2 == 65 && !artifact.IsWornByCharacter())
			{
				Globals.Engine.PrintEffectDesc(45);
				
				GotoCleanup = true;

				goto Cleanup;
			}

			artifact = Globals.ADB[14];

			Debug.Assert(artifact != null);

			if (Globals.GameState.Ro == 84 && Globals.GameState.R2 == 94)
			{
				// If descend pit w/ mgk boots, write effect

				if (artifact.IsWornByCharacter())
				{
					Globals.Engine.PrintEffectDesc(47);
				}

				// If descend pit w/out mgk boots, fall to death

				else
				{
					Globals.Out.Write("{0}It looks dangerous, try to climb down anyway (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						Globals.Engine.PrintEffectDesc(46);

						Globals.GameState.Die = 1;

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});
					}

					GotoCleanup = true;
				}

				goto Cleanup;
			}

			if (Globals.GameState.Ro == 94 && Globals.GameState.R2 == 84)
			{
				// If ascend pit w/ mgk boots, write effect

				if (artifact.IsWornByCharacter())
				{
					Globals.Engine.PrintEffectDesc(48);
				}

				// Cannot go up the pit if not wearing mgk boots

				else
				{
					Globals.Out.Print("The ceiling is too high to climb back up!");

					GotoCleanup = true;
				}

				goto Cleanup;
			}

		Cleanup:

			;
		}

		protected override void ProcessEvents01()
		{
			RetCode rc;

			if (Globals.GameState.R2 == Constants.DirectionExit)
			{
				// Successful adventure means the necromancer (22) is dead and Lady Mirabelle (26) is alive and exiting with player

				var monster = Globals.MDB[22];

				Debug.Assert(monster != null);

				var vanquished = monster.IsInLimbo();

				monster = Globals.MDB[26];

				Debug.Assert(monster != null);

				var rescued = monster.Location == Globals.GameState.Ro;

				if (!vanquished || !rescued)
				{
					Globals.Out.Print("You have not succeeded in your quest!");

					if (!vanquished)
					{
						Globals.Out.Print(" * The evil force here has not been vanquished");
					}

					if (!rescued)
					{
						Globals.Out.Print(" * Lady Mirabelle has not been rescued");
					}
				}
				else
				{
					Globals.Out.Print("YOU HAVE SUCCEEDED IN YOUR QUEST!  CONGRATULATIONS!");
				}

				Globals.Out.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					PrintRideOffIntoSunset();

					Globals.GameState.Die = 0;

					Globals.ExitType = Enums.ExitType.FinishAdventure;

					Globals.MainLoop.ShouldShutdown = true;
				}
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
