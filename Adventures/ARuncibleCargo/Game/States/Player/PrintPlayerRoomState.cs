
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(long eventType)
		{
			RetCode rc;

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeBeforePlayerRoomPrint && ShouldPreTurnProcess())
			{
				var characterMonster = Globals.MDB[gameState.Cm];

				Debug.Assert(characterMonster != null);

				var room = characterMonster.GetInRoom();

				Debug.Assert(room != null);

				//   Special events part 1
				//  -----------------------

				// Move from dream to fire

				if (room.Uid < 7 && gameState.DreamCounter >= 13)
				{
					var lookCommand = Globals.LastCommand as ILookCommand;

					if (lookCommand != null)
					{
						Globals.Engine.PrintPlayerRoom();
					}

					// Nothing in the dream affects the real world; revert game state now that player is awake

					var filesetTable = Globals.CloneInstance(Globals.Database.FilesetTable);

					Debug.Assert(filesetTable != null);

					var gameState01 = Globals.CloneInstance(gameState);

					Debug.Assert(gameState01 != null);

					rc = Globals.PopDatabase();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rc = Globals.RestoreDatabase(Constants.SnapshotFileName);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Database.FilesetTable = filesetTable;

					Globals.Config = Globals.Engine.GetConfig();

					Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault();

					Globals.Module = Globals.Engine.GetModule();

					Globals.GameState = Globals.Engine.GetGameState();

					gameState = Globals.GameState as Framework.IGameState;

					gameState.PookaMet = Globals.CloneInstance(gameState01.PookaMet);

					gameState.Ro = 7;

					gameState.R2 = 7;

					gameState.R3 = 7;

					gameState.Vr = gameState01.Vr;

					gameState.Vm = gameState01.Vm;

					gameState.Va = gameState01.Va;

					gameState.PauseCombatMs = gameState01.PauseCombatMs;

					characterMonster = Globals.MDB[gameState.Cm];

					room = Globals.RDB[gameState.Ro];

					characterMonster.SetInRoom(room);

					Globals.Engine.PrintEffectDesc(7);
				}

				// Out the burning window

				if (room.Uid == 8 && !gameState.FireEscaped)
				{
					Globals.Engine.PrintEffectDesc(8);

					gameState.FireEscaped = true;
				}

				Eamon.Framework.Primitive.Classes.IArtifactCategory ac = null;

				// Shop doors

				if (gameState.R3 == 19 && (room.Uid == 12 || room.Uid == 20))
				{
					gameState.R3 = 12;

					var shopDoorArtifact = Globals.ADB[room.Uid == 20 ? 136 : 17];

					Debug.Assert(shopDoorArtifact != null);

					ac = shopDoorArtifact.DoorGate;

					Debug.Assert(ac != null);

					ac.SetOpen(false);

					Globals.Engine.PrintEffectDesc(48);
				}

				// Out the Window of Ill Repute

				if (gameState.R3 == 18 && room.Uid == 50 && !gameState.AmazonMet)
				{
					Globals.Engine.PrintEffectDesc(47);

					gameState.AmazonMet = true;
				}

				// Bandit camp

				if (room.Uid == 59 && !gameState.CampEntered)
				{
					gameState.R3 *= gameState.CargoInRoom;

					Globals.Engine.PrintEffectDesc(68);

					gameState.CampEntered = true;
				}

				var larkspurMonster = Globals.MDB[36];

				Debug.Assert(larkspurMonster != null);

				// Meet Larkspur

				if (room.Uid == 88 && larkspurMonster.IsInRoom(room) && !larkspurMonster.Seen)
				{
					Globals.Engine.PrintEffectDesc(92);
				}

				var lilMonster = Globals.MDB[37];

				Debug.Assert(lilMonster != null);

				var cargoArtifact = Globals.ADB[129];

				Debug.Assert(cargoArtifact != null);

				// Lil steals Runcible Cargo

				if (room.Uid != 102 && room.Uid != 43 && (room.Uid < 86 || room.Uid > 88))
				{
					if ((cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByCharacter()) && lilMonster.IsInRoom(room))
					{
						cargoArtifact.SetCarriedByMonster(lilMonster);

						Globals.Engine.PrintEffectDesc(119);

						lilMonster.Friendliness = Friendliness.Enemy;

						lilMonster.OrigFriendliness = (Friendliness)100;
					}
				}

				var princeMonster = Globals.MDB[38];

				Debug.Assert(princeMonster != null);

				// The Prince would like the Cargo, please

				if (room.Uid == 96 && princeMonster.IsInRoom(room) && !gameState.PrinceMet)
				{
					gameState.R3 = 96;

					Globals.Engine.PrintEffectDesc(125);

					gameState.PrinceMet = true;
				}

				if (room.Uid == 96 && gameState.R3 == 95 && princeMonster.IsInRoom(room) && princeMonster.Friendliness > Friendliness.Enemy && !cargoArtifact.IsCarriedByMonster(princeMonster) && gameState.PrinceMet)
				{
					gameState.R3 = 96;

					Globals.Engine.PrintEffectDesc(127);
				}

				var guardsMonster = Globals.MDB[39];

				Debug.Assert(guardsMonster != null);

				var gatesArtifact = Globals.ADB[137];

				Debug.Assert(gatesArtifact != null);

				// Gates of Frukendorf slam shut

				if (room.Uid == 93 && cargoArtifact.IsCarriedByMonster(princeMonster) && princeMonster.Friendliness > Friendliness.Enemy && !guardsMonster.IsInLimbo())
				{
					ac = gatesArtifact.DoorGate;

					Debug.Assert(ac != null);

					if (ac.IsOpen())
					{
						ac.SetOpen(false);

						Globals.Engine.PrintEffectDesc(140);
					}
				}

				ac = cargoArtifact.InContainer;

				Debug.Assert(ac != null);

				// Cargo open counter

				if ((cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByCharacter()) && ac.IsOpen())
				{
					gameState.CargoOpenCounter++;

					if (gameState.CargoOpenCounter == 3)
					{
						ac.SetOpen(false);

						Globals.Engine.PrintEffectDesc(130);
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
