
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeCommandPromptPrint && ShouldPreTurnProcess())
			{
				var characterMonster = gMDB[gGameState.Cm];

				Debug.Assert(characterMonster != null);

				var room = characterMonster.GetInRoom();

				Debug.Assert(room != null);

				var efreetiMonster = gMDB[50];

				Debug.Assert(efreetiMonster != null);

				// Efreeti goes poof

				if (!efreetiMonster.IsInLimbo() && (!efreetiMonster.IsInRoom(room) || !efreetiMonster.CheckNBTLHostility()) && gEngine.RollDice(1, 100, 0) <= 50)
				{
					if (efreetiMonster.IsInRoom(room) && room.IsLit())
					{
						gOut.Print("{0}{1} vanishes into thin air.", efreetiMonster.GetTheName(true), efreetiMonster.Friendliness == Friendliness.Friend ? ", seeing that you aren't in any immediate danger," : "");
					}

					efreetiMonster.SetInLimbo();
				}

				var cloakAndCowlArtifact = gADB[44];

				Debug.Assert(cloakAndCowlArtifact != null);

				// Dark hood and small glade

				if (cloakAndCowlArtifact.IsInLimbo())
				{
					var darkHoodMonster = gMDB[21];

					Debug.Assert(darkHoodMonster != null);

					if (!darkHoodMonster.IsInLimbo())
					{
						var darkHoodInPlayerRoom = darkHoodMonster.IsInRoom(room);

						var darkHoodVanishes = false;

						if (gGameState.IsDayTime())
						{
							darkHoodMonster.SetInLimbo();

							darkHoodVanishes = true;
						}
						else if (room.Uid != 23 && !darkHoodMonster.IsInRoomUid(23))
						{
							darkHoodMonster.SetInRoomUid(23);

							darkHoodVanishes = true;
						}

						if (darkHoodInPlayerRoom && darkHoodVanishes)
						{
							gOut.Print("{0} suddenly vanishes, seemingly into thin air.", darkHoodMonster.GetTheName(true));
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
