
// PrintPlayerRoomState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using ARuncibleCargo.Framework;
using ARuncibleCargo.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPrintPlayerRoomState))]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		protected override void ProcessEvents()
		{
			if (ShouldPreTurnProcess())
			{
				RetCode rc;

				var gameState = Globals.GameState as IGameState;

				Debug.Assert(gameState != null);

				var monster = Globals.MDB[gameState.Cm];

				Debug.Assert(monster != null);

				var room = monster.GetInRoom();

				Debug.Assert(room != null);

				//   Special events part 1
				//  -----------------------

				// Move from dream to fire

				if (room.Uid < 7 && gameState.DreamCounter >= 13)
				{
					var lookCommand = Globals.LastCommand as ILookCommand;

					if (lookCommand != null)
					{
						PrintPlayerRoom();
					}

					// Nothing in the dream affects the real world; revert game state now that player is awake

					var filesetTable = Globals.CloneInstance(Globals.Database.FilesetTable);

					Debug.Assert(filesetTable != null);

					var gameState01 = Globals.CloneInstance(gameState);

					Debug.Assert(gameState01 != null);

					rc = Globals.PopDatabase();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rc = Globals.RestoreDatabase(Globals.GetPrefixedFileName(Constants.SnapshotFileName));

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Database.FilesetTable = filesetTable;

					Globals.Config = Globals.Engine.GetConfig();

					Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault();

					Globals.Module = Globals.Engine.GetModule();

					Globals.GameState = Globals.Engine.GetGameState();

					gameState = Globals.GameState as IGameState;

					gameState.PookaMet = Globals.CloneInstance(gameState01.PookaMet);

					gameState.Ro = 7;

					gameState.R2 = 7;

					gameState.R3 = 7;

					gameState.Lt = 1;

					gameState.Vr = gameState01.Vr;

					gameState.Vm = gameState01.Vm;

					gameState.Va = gameState01.Va;

					monster = Globals.MDB[gameState.Cm];

					room = Globals.RDB[gameState.Ro];

					monster.SetInRoom(room);

					Globals.Engine.CheckEnemies();

					Globals.Engine.PrintEffectDesc(7);
				}

				// Out the burning window

				if (room.Uid == 8 && !gameState.FireEscaped)
				{
					Globals.Engine.PrintEffectDesc(8);

					gameState.FireEscaped = true;
				}

				Eamon.Framework.Primitive.Classes.IArtifactClass ac = null;

				Eamon.Framework.IArtifact artifact = null;

				// Shop doors

				if (gameState.R3 == 19 && (room.Uid == 12 || room.Uid == 20))
				{
					gameState.R3 = 12;

					artifact = Globals.ADB[room.Uid == 20 ? 136 : 17];

					Debug.Assert(artifact != null);

					ac = artifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

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

				var monster01 = Globals.MDB[36];

				Debug.Assert(monster01 != null);

				// Meet Larkspur

				if (room.Uid == 88 && monster01.IsInRoom(room) && !monster01.Seen)
				{
					Globals.Engine.PrintEffectDesc(92);
				}

				monster01 = Globals.MDB[37];

				Debug.Assert(monster01 != null);

				artifact = Globals.ADB[129];

				Debug.Assert(artifact != null);

				// Lil steals Runcible Cargo

				if (room.Uid != 102 && room.Uid != 43 && (room.Uid < 86 || room.Uid > 88))
				{
					if ((artifact.IsInRoom(room) || artifact.IsCarriedByCharacter()) && monster01.IsInRoom(room))
					{
						Globals.Engine.RemoveWeight(artifact);

						artifact.SetCarriedByMonster(monster01);

						Globals.Engine.PrintEffectDesc(119);

						monster01.Friendliness = Enums.Friendliness.Enemy;

						monster01.OrigFriendliness = (Enums.Friendliness)100;

						Globals.Engine.CheckEnemies();
					}
				}

				monster01 = Globals.MDB[38];

				Debug.Assert(monster01 != null);

				// The Prince would like the Cargo, please

				if (room.Uid == 96 && monster01.IsInRoom(room) && !gameState.PrinceMet)
				{
					gameState.R3 = 96;

					Globals.Engine.PrintEffectDesc(125);

					gameState.PrinceMet = true;
				}

				if (room.Uid == 96 && gameState.R3 == 95 && monster01.IsInRoom(room) && monster01.Friendliness > Enums.Friendliness.Enemy && !artifact.IsCarriedByMonster(monster01) && gameState.PrinceMet)
				{
					gameState.R3 = 96;

					Globals.Engine.PrintEffectDesc(127);
				}

				var monster02 = Globals.MDB[39];

				Debug.Assert(monster02 != null);

				var artifact01 = Globals.ADB[137];

				Debug.Assert(artifact01 != null);

				// Gates of Frukendorf slam shut

				if (room.Uid == 93 && artifact.IsCarriedByMonster(monster01) && monster01.Friendliness > Enums.Friendliness.Enemy && !monster02.IsInLimbo())
				{
					ac = artifact01.GetArtifactClass(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac != null);

					if (ac.IsOpen())
					{
						ac.SetOpen(false);

						Globals.Engine.PrintEffectDesc(140);
					}
				}

				ac = artifact.GetArtifactClass(Enums.ArtifactType.Container);

				Debug.Assert(ac != null);

				// Cargo open counter

				if ((artifact.IsInRoom(room) || artifact.IsCarriedByCharacter()) && ac.IsOpen())
				{
					gameState.CargoOpenCounter++;

					if (gameState.CargoOpenCounter == 3)
					{
						ac.SetOpen(false);

						Globals.Engine.PrintEffectDesc(130);
					}
				}
			}

			base.ProcessEvents();
		}
	}
}
