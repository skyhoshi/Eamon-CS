
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
						gOut.Print("The efreeti{0} vanishes into thin air.", efreetiMonster.Friendliness == Friendliness.Friend ? ", seeing that you aren't in any immediate danger," : "");
					}

					efreetiMonster.SetInLimbo();
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
