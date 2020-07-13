﻿
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public virtual bool EnemiesInRoom()
		{
			var result = false;

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;

				result = true;
			}

			return result;
		}

		public virtual void TravelByTrain(long newRoomUid, long effectUid)
		{
			// Train Routine

			var newRoom = gRDB[newRoomUid];

			Debug.Assert(newRoom != null);

			var effect = gEDB[effectUid];

			Debug.Assert(effect != null);

			gEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

			NextState = Globals.CreateInstance<IAfterPlayerMoveState>();

			GotoCleanup = true;
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeBeforePlayerSayTextPrint)
			{
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

				if (found && (gGameState.Ro == 28 || (gGameState.Ro > 88 && gGameState.Ro < 92)))
				{
					gOut.Print("Thank you for flying Frank Black Airlines!");
				}
			}
			else if (eventType == PpeAfterPlayerSay)
			{
				var princeMonster = gMDB[38];

				Debug.Assert(princeMonster != null);

				var cargoArtifact = gADB[129];

				Debug.Assert(cargoArtifact != null);

				//                     Ye Olde Eamon Railroad
				//                    ------------------------

				// Verify Runcible Cargo before allowing travel to Frukendorf

				if ((ActorRoom.Uid == 28 || ActorRoom.Uid == 89 || ActorRoom.Uid == 90) && string.Equals(ProcessedPhrase, "frukendorf", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					if (!cargoArtifact.IsInRoom(ActorRoom) && !cargoArtifact.IsCarriedByCharacter())
					{
						gEngine.PrintEffectDesc(107);

						NextState = Globals.CreateInstance<IStartState>();

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
					if (!cargoArtifact.IsCarriedByMonster(princeMonster))
					{
						gEngine.PrintEffectDesc(106);

						NextState = Globals.CreateInstance<IStartState>();

						GotoCleanup = true;

						goto Cleanup;
					}

					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					if (string.Equals(ProcessedPhrase, "dodge", StringComparison.OrdinalIgnoreCase) || string.Equals(ProcessedPhrase, "mudville", StringComparison.OrdinalIgnoreCase))
					{
						gEngine.PrintEffectDesc(141);

						NextState = Globals.CreateInstance<IStartState>();

						GotoCleanup = true;

						goto Cleanup;
					}

					// Return to Main Hall after capitulating to the Bandits

					gOut.Print("You begin your journey home...");

					gOut.Print("{0}", Globals.LineSep);

					gEngine.PrintEffectDesc(145);

					Globals.In.KeyPress(Globals.Buf);

					gGameState.Die = 0;

					Globals.ExitType = ExitType.FinishAdventure;

					Globals.MainLoop.ShouldShutdown = true;

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}
			}

			base.PlayerProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
