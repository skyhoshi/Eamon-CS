
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using ARuncibleCargo.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IAfterPlayerMoveState))]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		protected override void ProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			// Boat stuff (an Eamon tradition)

			if ((gameState.Ro == 97 && gameState.R3 == 31) || (gameState.Ro == 99 && gameState.R3 == 32) || (gameState.Ro == 101 && gameState.R3 == 67))
			{
				Globals.Engine.PrintEffectDesc(113);
			}

			if ((gameState.Ro == 31 && gameState.R3 == 97) || (gameState.Ro == 32 && gameState.R3 == 99) || (gameState.Ro == 67 && gameState.R3 == 101))
			{
				Globals.Engine.PrintEffectDesc(114);
			}

			// Sync contents of water rooms

			var oldRoom = Globals.RDB[gameState.R3] as IRoom;

			var newRoom = Globals.RDB[gameState.Ro] as IRoom;

			if (oldRoom != null && oldRoom.IsWaterRoom() && newRoom != null && newRoom.IsWaterRoom())
			{
				Globals.Engine.TransportRoomContentsBetweenRooms(oldRoom, newRoom, false);
			}

			// Out the Window of Ill Repute

			if (gameState.Ro == 21 && gameState.R3 == 50)
			{
				Globals.Engine.PrintEffectDesc(49);
			}

			base.ProcessEvents();
		}
	}
}
