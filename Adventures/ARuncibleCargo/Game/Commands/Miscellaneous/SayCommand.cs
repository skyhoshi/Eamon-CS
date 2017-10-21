
// SayCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using ARuncibleCargo.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ISayCommand))]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		protected virtual bool EnemiesInRoom()
		{
			var result = false;

			if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;

				result = true;
			}

			return result;
		}

		protected virtual void TravelByTrain(long newRoomUid, long effectUid)
		{
			// Train Routine

			var newRoom = Globals.RDB[newRoomUid];

			Debug.Assert(newRoom != null);

			var effect = Globals.EDB[effectUid];

			Debug.Assert(effect != null);

			Globals.Engine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

			NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();

			GotoCleanup = true;
		}

		protected override void PlayerProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var found = false;

			// Fly FBA today and get there faster!

			if (string.Equals(ProcessedPhrase, "*d", StringComparison.OrdinalIgnoreCase))
			{
				PrintedPhrase = "Dodge.";

				ProcessedPhrase = "dodge";

				found = true;
			}

			if (string.Equals(ProcessedPhrase, "*f", StringComparison.OrdinalIgnoreCase))
			{
				PrintedPhrase = "Frukendorf.";

				ProcessedPhrase = "frukendorf";

				found = true;
			}

			if (string.Equals(ProcessedPhrase, "*h", StringComparison.OrdinalIgnoreCase))
			{
				PrintedPhrase = "Main Hall.";

				ProcessedPhrase = "main hall";

				found = true;
			}

			if (string.Equals(ProcessedPhrase, "*m", StringComparison.OrdinalIgnoreCase))
			{
				PrintedPhrase = "Mudville.";

				ProcessedPhrase = "mudville";

				found = true;
			}

			if (found && (gameState.Ro == 28 || (gameState.Ro > 88 && gameState.Ro < 92)))
			{
				Globals.Out.Write("{0}Thank you for flying Frank Black Airlines!{0}", Environment.NewLine);
			}

			base.PlayerProcessEvents();
		}

		protected override void PlayerProcessEvents01()
		{
			var monster = Globals.MDB[38];

			Debug.Assert(monster != null);

			var artifact = Globals.ADB[129];

			Debug.Assert(artifact != null);

			//                     Ye Olde Eamon Railroad
			//                    ------------------------

			// Verify Runcible Cargo before allowing travel to Frukendorf

			if ((ActorRoom.Uid == 28 || ActorRoom.Uid == 89 || ActorRoom.Uid == 90) && string.Equals(ProcessedPhrase, "frukendorf", StringComparison.OrdinalIgnoreCase))
			{
				if (EnemiesInRoom())
				{
					goto Cleanup;
				}

				if (!artifact.IsInRoom(ActorRoom) && !artifact.IsCarriedByCharacter())
				{
					Globals.Engine.PrintEffectDesc(107);

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				TravelByTrain(91, 109);

				goto Cleanup;
			}

			// Route 100: Main Hall Station

			if (ActorRoom.Uid == 28)
			{
				if (string.Equals(ProcessedPhrase, "dodge", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					TravelByTrain(89, 99);

					goto Cleanup;
				}

				if (string.Equals(ProcessedPhrase, "mudville", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					TravelByTrain(90, 103);

					goto Cleanup;
				}
			}

			// Route 13: Dodge Station

			if (ActorRoom.Uid == 89)
			{
				if (string.Equals(ProcessedPhrase, "main hall", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					TravelByTrain(28, 100);

					goto Cleanup;
				}

				if (string.Equals(ProcessedPhrase, "mudville", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					TravelByTrain(90, 101);

					goto Cleanup;
				}
			}

			// Route 0: Mudville Station

			if (ActorRoom.Uid == 90)
			{
				if (string.Equals(ProcessedPhrase, "dodge", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					TravelByTrain(89, 102);

					goto Cleanup;
				}

				if (string.Equals(ProcessedPhrase, "main hall", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					TravelByTrain(28, 104);

					goto Cleanup;
				}
			}

			// Route 66: Frukendorf Station

			if (ActorRoom.Uid == 91 && (string.Equals(ProcessedPhrase, "main hall", StringComparison.OrdinalIgnoreCase) || string.Equals(ProcessedPhrase, "dodge", StringComparison.OrdinalIgnoreCase) || string.Equals(ProcessedPhrase, "mudville", StringComparison.OrdinalIgnoreCase)))
			{
				if (!artifact.IsCarriedByMonster(monster))
				{
					Globals.Engine.PrintEffectDesc(106);

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				if (EnemiesInRoom())
				{
					goto Cleanup;
				}

				if (string.Equals(ProcessedPhrase, "dodge", StringComparison.OrdinalIgnoreCase) || string.Equals(ProcessedPhrase, "mudville", StringComparison.OrdinalIgnoreCase))
				{
					Globals.Engine.PrintEffectDesc(141);

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// Return to Main Hall after capitulating to the Bandits

				Globals.Out.Write("{0}You begin your journey home...{0}", Environment.NewLine);

				Globals.Out.Write("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

				Globals.Engine.PrintEffectDesc(145);

				Globals.In.KeyPress(Globals.Buf);

				Globals.GameState.Die = 0;

				Globals.ExitType = Enums.ExitType.FinishAdventure;

				Globals.MainLoop.ShouldShutdown = true;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;

				goto Cleanup;
			}

			base.PlayerProcessEvents01();

		Cleanup:

			;
		}
	}
}
