
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeCommandPromptPrint && ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as Framework.IGameState;

				Debug.Assert(gameState != null);

				var characterMonster = Globals.MDB[gameState.Cm];

				Debug.Assert(characterMonster != null);

				var room = characterMonster.GetInRoom();

				Debug.Assert(room != null);

				//   Special events part 2
				//  -----------------------

				// Dream effects

				if (room.Uid < 7)
				{
					if (gameState.DreamCounter == 4)
					{
						Globals.Engine.PrintEffectDesc(1);
					}

					if (gameState.DreamCounter == 8)
					{
						Globals.Engine.PrintEffectDesc(2);
					}

					if (gameState.DreamCounter == 12)
					{
						Globals.Engine.PrintEffectDesc(3);
					}

					gameState.DreamCounter++;
				}

				var fortunetellerArtifact = Globals.ADB[101];

				Debug.Assert(fortunetellerArtifact != null);

				// Swarmy

				if (room.Uid == 66 && fortunetellerArtifact.IsInRoom(room))
				{
					if (gameState.SwarmyCounter == 1)
					{
						Globals.Engine.PrintEffectDesc(91);
					}

					gameState.SwarmyCounter++;

					if (gameState.SwarmyCounter > 3)
					{
						gameState.SwarmyCounter = 1;
					}
				}

				// In the burning room

				if (room.Uid == 7)
				{
					Globals.Engine.PrintEffectDesc(13);

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = characterMonster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 1);

					if (Globals.GameState.Die > 0)
					{
						GotoCleanup = true;

						goto Cleanup;
					}
				}

				var hokasTokasMonster = Globals.MDB[4];

				Debug.Assert(hokasTokasMonster != null);

				// Hokas scolds you

				if (room.Uid == 8 && gameState.R3 == 9 && hokasTokasMonster.IsInRoom(room))
				{
					hokasTokasMonster.SetInRoomUid(1);

					Globals.Engine.CheckEnemies();

					Globals.Engine.PrintEffectDesc(11);

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				var ovenArtifact = Globals.ADB[80];

				Debug.Assert(ovenArtifact != null);

				var ac = ovenArtifact.Container;

				Debug.Assert(ac != null);

				var billMonster = Globals.MDB[23];

				Debug.Assert(billMonster != null);

				// Bill in oven

				if (room.Uid == 55 && !ac.IsOpen() && !billMonster.Seen)
				{
					Globals.Engine.PrintEffectDesc(52);
				}

				// Bandit camp

				if (room.Uid == 59 && gameState.R3 == 58)
				{
					gameState.R3 = 59;

					Globals.Engine.PrintEffectDesc(71 + gameState.CargoInRoom);
				}

				var westernDoorArtifact = Globals.ADB[112];

				Debug.Assert(westernDoorArtifact != null);

				var easternDoorArtifact = Globals.ADB[113];

				Debug.Assert(easternDoorArtifact != null);

				ac = easternDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				// University doors

				if (room.Uid == 74 && billMonster.IsInRoom(room) && ac.GetKeyUid() == -1)
				{
					ac.SetKeyUid(0);

					ac.SetOpen(true);

					Globals.Engine.PrintEffectDesc(86);
				}

				if (room.Uid == 74 && (gameState.R3 == 85 || gameState.R3 == 79))
				{
					var doorArtifact = easternDoorArtifact;

					var effectUid = 87L;

					if (gameState.R3 == 79)
					{
						doorArtifact = westernDoorArtifact;

						effectUid = 152;
					}

					gameState.R3 = room.Uid;

					Debug.Assert(doorArtifact != null);

					ac = doorArtifact.DoorGate;

					Debug.Assert(ac != null);

					if (!ac.IsOpen())
					{
						ac.SetKeyUid(0);

						ac.SetOpen(true);

						Globals.Engine.PrintEffectDesc(effectUid);
					}
				}

				var jailCellArtifact = Globals.ADB[41];

				Debug.Assert(jailCellArtifact != null);

				ac = jailCellArtifact.Container;

				Debug.Assert(ac != null);

				var lilMonster = Globals.MDB[37];

				Debug.Assert(lilMonster != null);

				// Lil in jail

				if (room.Uid == 102 && !ac.IsOpen() && !lilMonster.Seen)
				{
					Globals.Engine.PrintEffectDesc(122);
				}

				var amazonMonster = Globals.MDB[22];

				Debug.Assert(amazonMonster != null);

				// Amazon and Bill

				if (amazonMonster.IsInRoom(room) && billMonster.IsInRoom(room) && !gameState.BillAndAmazonMeet)
				{
					Globals.Engine.PrintEffectDesc(53);

					gameState.BillAndAmazonMeet = true;
				}

				var princeAndGuardsDead = false;

				var commanderMonster = Globals.MDB[27];

				Debug.Assert(commanderMonster != null);

				var soldiersMonster = Globals.MDB[28];

				Debug.Assert(soldiersMonster != null);

				// Bandit Commander and all soldiers are dead!

				var commanderAndSoldiersDead = commanderMonster.IsInLimbo() && soldiersMonster.IsInLimbo();

				if (!commanderAndSoldiersDead)
				{
					var princeMonster = Globals.MDB[38];

					Debug.Assert(princeMonster != null);

					var guardsMonster = Globals.MDB[39];

					Debug.Assert(guardsMonster != null);

					// Bandit Prince and all Praetorian Guards are dead!

					princeAndGuardsDead = princeMonster.IsInLimbo() && guardsMonster.IsInLimbo();
				}

				if (commanderAndSoldiersDead || princeAndGuardsDead)
				{
					var effectUid = commanderAndSoldiersDead ? 60L : 142L;

					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Engine.PrintEffectDesc(effectUid);

					Globals.In.KeyPress(Globals.Buf);

					gameState.Die = 0;

					Globals.ExitType = Enums.ExitType.FinishAdventure;

					Globals.MainLoop.ShouldShutdown = true;

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// Amazon and Bill warn about Lil

				if (lilMonster.IsInRoom(room))       // && (room.Uid == 18 || room.Uid == 21 || room.Uid == 38 || room.Uid == 53)
				{
					if (!gameState.AmazonLilWarning && amazonMonster.IsInRoom(room) && room.Uid != 50 && room.IsLit())
					{
						Globals.Engine.PrintEffectDesc(117);

						gameState.AmazonLilWarning = true;
					}

					if (!gameState.BillLilWarning && billMonster.IsInRoom(room) && room.Uid != 55 && room.IsLit())
					{
						Globals.Engine.PrintEffectDesc(118);

						gameState.BillLilWarning = true;
					}
				}

				var explosiveDeviceArtifact = Globals.ADB[43];

				Debug.Assert(explosiveDeviceArtifact != null);

				var remoteDetonatorArtifact = Globals.ADB[45];

				Debug.Assert(remoteDetonatorArtifact != null);

				// Bill (or Amazon) hint at using explosives

				if (room.Uid == 92 && (explosiveDeviceArtifact.IsInRoom(room) || explosiveDeviceArtifact.IsCarriedByCharacter()) && (remoteDetonatorArtifact.IsInRoom(room) || remoteDetonatorArtifact.IsCarriedByCharacter()))
				{
					var effectUid = 0L;

					if (amazonMonster.IsInRoom(room) && amazonMonster.Friendliness > Enums.Friendliness.Enemy)
					{
						effectUid = 124;
					}

					if (billMonster.IsInRoom(room) && billMonster.Friendliness > Enums.Friendliness.Enemy)
					{
						effectUid = 123;
					}

					if (effectUid > 0 && !gameState.Explosive)
					{
						Globals.Engine.PrintEffectDesc(effectUid);

						gameState.Explosive = true;
					}
				}

				Eamon.Framework.Primitive.Classes.IArtifactCategory ac01 = null;

				// Maintenance grate, sewer grate, and (Barney) Rubble

				var doubleDoors = Globals.DoubleDoors.Where(dd => dd.RoomUid == room.Uid).ToList();

				foreach (var dd in doubleDoors)
				{
					var doorArtifact = Globals.ADB[dd.ArtifactUid1];

					Debug.Assert(doorArtifact != null);

					var doorArtifact01 = Globals.ADB[dd.ArtifactUid2];

					Debug.Assert(doorArtifact01 != null);

					ac = doorArtifact.DoorGate;

					Debug.Assert(ac != null);

					ac01 = doorArtifact01.DoorGate;

					Debug.Assert(ac01 != null);

					doorArtifact01.Seen = doorArtifact.Seen;

					doorArtifact01.StateDesc = Globals.CloneInstance(doorArtifact.StateDesc);

					ac01.Field2 = ac.Field2;

					ac01.Field3 = ac.Field3;
				}
			}
			
			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
