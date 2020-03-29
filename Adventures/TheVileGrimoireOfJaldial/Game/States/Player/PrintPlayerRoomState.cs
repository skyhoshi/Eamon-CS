
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforePlayerRoomPrint && ShouldPreTurnProcess())
			{
				var characterMonster = gMDB[gGameState.Cm];

				Debug.Assert(characterMonster != null);

				var room = characterMonster.GetInRoom();

				Debug.Assert(room != null);

				var cloakAndCowlArtifact = gADB[44];

				Debug.Assert(cloakAndCowlArtifact != null);

				// Dark hood and small glade

				if (cloakAndCowlArtifact.IsInLimbo())
				{
					var darkHoodMonster = gMDB[21];

					Debug.Assert(darkHoodMonster != null);

					if (darkHoodMonster.IsInLimbo() && gGameState.IsNightTime())
					{
						darkHoodMonster.SetInRoomUid(23);

						if (darkHoodMonster.IsInRoom(room))
						{
							gOut.Print("A mysterious figure suddenly appears, seemingly out of thin air.");
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
