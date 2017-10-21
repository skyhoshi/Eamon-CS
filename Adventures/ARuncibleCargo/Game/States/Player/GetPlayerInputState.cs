
// GetPlayerInputState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using ARuncibleCargo.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IGetPlayerInputState))]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		protected override void ProcessEvents()
		{
			if (ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as IGameState;

				Debug.Assert(gameState != null);

				var monster = Globals.MDB[gameState.Cm];

				Debug.Assert(monster != null);

				var room = monster.GetInRoom();

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

				var artifact = Globals.ADB[101];

				Debug.Assert(artifact != null);

				// Swarmy

				if (room.Uid == 66 && artifact.IsInRoom(room))
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

					var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = monster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 1);

					if (Globals.GameState.Die > 0)
					{
						GotoCleanup = true;

						goto Cleanup;
					}
				}

				var monster01 = Globals.MDB[4];

				Debug.Assert(monster01 != null);

				// Hokas scolds you

				if (room.Uid == 8 && gameState.R3 == 9 && monster01.IsInRoom(room))
				{
					monster01.SetInRoomUid(1);

					Globals.Engine.CheckEnemies();

					Globals.Engine.PrintEffectDesc(11);

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				artifact = Globals.ADB[80];

				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactClass(Enums.ArtifactType.Container);

				Debug.Assert(ac != null);

				monster01 = Globals.MDB[23];

				Debug.Assert(monster01 != null);

				// Bill in oven

				if (room.Uid == 55 && !ac.IsOpen() && !monster01.Seen)
				{
					Globals.Engine.PrintEffectDesc(52);
				}

				// Bandit camp

				if (room.Uid == 59 && gameState.R3 == 58)
				{
					gameState.R3 = 59;

					Globals.Engine.PrintEffectDesc(71 + gameState.CargoInRoom);
				}

				artifact = Globals.ADB[113];

				Debug.Assert(artifact != null);

				ac = artifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				// University doors

				if (room.Uid == 74 && monster01.IsInRoom(room) && ac.GetKeyUid() == -1)
				{
					ac.SetKeyUid(0);

					ac.SetOpen(true);

					Globals.Engine.PrintEffectDesc(86);
				}

				if (room.Uid == 74 && (gameState.R3 == 85 || gameState.R3 == 79))
				{
					var artUid = 113L;

					var effectUid = 87L;

					if (gameState.R3 == 79)
					{
						artUid = 112;

						effectUid = 152;
					}

					gameState.R3 = room.Uid;

					artifact = Globals.ADB[artUid];

					Debug.Assert(artifact != null);

					ac = artifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac != null);

					if (!ac.IsOpen())
					{
						ac.SetKeyUid(0);

						ac.SetOpen(true);

						Globals.Engine.PrintEffectDesc(effectUid);
					}
				}

				artifact = Globals.ADB[41];

				Debug.Assert(artifact != null);

				ac = artifact.GetArtifactClass(Enums.ArtifactType.Container);

				Debug.Assert(ac != null);

				monster01 = Globals.MDB[37];

				Debug.Assert(monster01 != null);

				// Lil in jail

				if (room.Uid == 102 && !ac.IsOpen() && !monster01.Seen)
				{
					Globals.Engine.PrintEffectDesc(122);
				}

				monster01 = Globals.MDB[22];

				Debug.Assert(monster01 != null);

				var monster02 = Globals.MDB[23];

				Debug.Assert(monster02 != null);

				// Amazon and Bill

				if (monster01.IsInRoom(room) && monster02.IsInRoom(room) && !gameState.BillAndAmazonMeet)
				{
					Globals.Engine.PrintEffectDesc(53);

					gameState.BillAndAmazonMeet = true;
				}

				var princeAndGuardsDead = false;

				monster01 = Globals.MDB[27];

				Debug.Assert(monster01 != null);

				monster02 = Globals.MDB[28];

				Debug.Assert(monster02 != null);

				// Bandit Commander and all soldiers are dead!

				var commanderAndSoldiersDead = monster01.IsInLimbo() && monster02.IsInLimbo();

				if (!commanderAndSoldiersDead)
				{
					monster01 = Globals.MDB[38];

					Debug.Assert(monster01 != null);

					monster02 = Globals.MDB[39];

					Debug.Assert(monster02 != null);

					// Bandit Prince and all Praetorian Guards are dead!

					princeAndGuardsDead = monster01.IsInLimbo() && monster02.IsInLimbo();
				}

				if (commanderAndSoldiersDead || princeAndGuardsDead)
				{
					var effectUid = commanderAndSoldiersDead ? 60L : 142L;

					Globals.Out.Write("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

					Globals.Engine.PrintEffectDesc(effectUid);

					Globals.In.KeyPress(Globals.Buf);

					gameState.Die = 0;

					Globals.ExitType = Enums.ExitType.FinishAdventure;

					Globals.MainLoop.ShouldShutdown = true;

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				monster01 = Globals.MDB[37];

				Debug.Assert(monster01 != null);

				// Amazon and Bill warn about Lil

				if (monster01.IsInRoom(room))       // && (room.Uid == 18 || room.Uid == 21 || room.Uid == 38 || room.Uid == 53)
				{
					monster02 = Globals.MDB[22];

					Debug.Assert(monster02 != null);

					if (!gameState.AmazonLilWarning && monster02.IsInRoom(room) && room.Uid != 50 && room.IsLit())
					{
						Globals.Engine.PrintEffectDesc(117);

						gameState.AmazonLilWarning = true;
					}

					monster02 = Globals.MDB[23];

					Debug.Assert(monster02 != null);

					if (!gameState.BillLilWarning && monster02.IsInRoom(room) && room.Uid != 55 && room.IsLit())
					{
						Globals.Engine.PrintEffectDesc(118);

						gameState.BillLilWarning = true;
					}
				}

				artifact = Globals.ADB[43];

				Debug.Assert(artifact != null);

				var artifact01 = Globals.ADB[45];

				Debug.Assert(artifact01 != null);

				// Bill (or Amazon) hint at using explosives

				if (room.Uid == 92 && (artifact.IsInRoom(room) || artifact.IsCarriedByCharacter()) && (artifact01.IsInRoom(room) || artifact01.IsCarriedByCharacter()))
				{
					var effectUid = 0L;

					monster01 = Globals.MDB[22];

					Debug.Assert(monster01 != null);

					if (monster01.IsInRoom(room) && monster01.Friendliness > Enums.Friendliness.Enemy)
					{
						effectUid = 124;
					}

					monster01 = Globals.MDB[23];

					Debug.Assert(monster01 != null);

					if (monster01.IsInRoom(room) && monster01.Friendliness > Enums.Friendliness.Enemy)
					{
						effectUid = 123;
					}

					if (effectUid > 0 && !gameState.Explosive)
					{
						Globals.Engine.PrintEffectDesc(effectUid);

						gameState.Explosive = true;
					}
				}

				Eamon.Framework.Primitive.Classes.IArtifactClass ac01 = null;

				// Maintenance grate, sewer grate, and (Barney) Rubble

				var doubleDoors = Globals.DoubleDoors.Where(dd => dd.RoomUid == room.Uid).ToList();

				foreach (var dd in doubleDoors)
				{
					artifact = Globals.ADB[dd.ArtifactUid1];

					Debug.Assert(artifact != null);

					artifact01 = Globals.ADB[dd.ArtifactUid2];

					Debug.Assert(artifact01 != null);

					ac = artifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac != null);

					ac01 = artifact01.GetArtifactClass(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac01 != null);

					artifact01.Seen = artifact.Seen;

					artifact01.StateDesc = Globals.CloneInstance(artifact.StateDesc);

					ac01.Field6 = ac.Field6;

					ac01.Field7 = ac.Field7;
				}
			}

			base.ProcessEvents();

		Cleanup:

			;
		}
	}
}
