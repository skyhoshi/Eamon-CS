
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterExtinguishLightSourceCheck)
			{
				var swampRoomUids = new long[] { 42, 43, 44, 45 };

				var room01 = Room as Framework.IRoom;

				Debug.Assert(room01 != null);

				var room03 = gRDB[gGameState.R3] as Framework.IRoom;

				Debug.Assert(room03 != null);

				// Traveling through the swamp at night or in dense fog with no light source - not advisable

				var odds = (room03.IsDimLightRoomWithoutGlowingMonsters() || room01.IsDimLightRoomWithoutGlowingMonsters()) && gGameState.Ls <= 0 ? 50 : 70;

				// Check for travel through the swamp

				if (gGameState.Ro != 19 && (swampRoomUids.Contains(gGameState.R3) || swampRoomUids.Contains(gGameState.Ro)) && gEngine.RollDice(1, 100, 0) > odds)
				{
					if (!gEngine.SaveThrow(0))
					{
						gEngine.PrintEffectDesc(91);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
					else
					{
						gEngine.PrintEffectDesc(94);
					}
				}

				var room03IsDimLightRoom = room03.IsDimLightRoom();

				// Check for foggy room

				gGameState.SetFoggyRoomWeatherIntensity(room01);

				var room01IsDimLightRoom = room01.IsDimLightRoom();

				// Clear Seen flags when transitioning between room types and dim light levels

				var roomTransition = (room03.IsGroundsRoom() && room01.IsCryptRoom()) || (room03.IsCryptRoom() && room01.IsGroundsRoom());

				var dimLightTransition = (room03IsDimLightRoom && !room01IsDimLightRoom) || (!room03IsDimLightRoom && room01IsDimLightRoom);

				if (roomTransition && dimLightTransition)       // TODO: verify teleportation works
				{
					var monsterList = gEngine.GetMonsterList(m => m.IsInRoom(room01));

					foreach (var m in monsterList)
					{
						var artifactList = m.GetContainedList(recurse: true);

						foreach (var a in artifactList)
						{
							if (!a.IsCharOwned)
							{
								a.Seen = false;
							}
						}

						if (!m.IsCharacterMonster())
						{
							m.Seen = false; 
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
