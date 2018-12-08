
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforePlayerRoomPrint && ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as Framework.IGameState;

				Debug.Assert(gameState != null);

				var characterMonster = Globals.MDB[gameState.Cm];

				Debug.Assert(characterMonster != null);

				var characterRoom = characterMonster.GetInRoom();

				Debug.Assert(characterRoom != null);

				// Events only occur in lit rooms

				if (characterRoom.IsLit())
				{
					// Kobolds appear (30% chance)

					if (!gameState.KoboldsAppear && characterRoom.Uid > 10 && characterRoom.Uid < 16)
					{
						var rl = Globals.Engine.RollDice(1, 100, 0);

						if (rl < 31)
						{
							for (var i = 6; i <= 9; i++)
							{
								var koboldMonster = Globals.MDB[i];

								Debug.Assert(koboldMonster != null);

								koboldMonster.SetInRoom(characterRoom);
							}

							Globals.Engine.CheckEnemies();

							gameState.KoboldsAppear = true;
						}
					}

					var zapfMonster = Globals.MDB[15];

					Debug.Assert(zapfMonster != null);

					var staffArtifact = Globals.ADB[33];

					Debug.Assert(staffArtifact != null);

					// Zapf the Conjurer brings in strangers (15% Chance)

					if (zapfMonster.IsInRoom(characterRoom) && zapfMonster.Seen && !Globals.Engine.CheckNBTLHostility(zapfMonster) && staffArtifact.IsCarriedByMonster(zapfMonster))
					{
						var rl = Globals.Engine.RollDice(1, 100, 0);

						if (rl < 16)
						{
							Globals.Engine.PrintEffectDesc(16);

							// Exclude character monster

							rl = Globals.Engine.RollDice(1, Globals.Database.MonsterTable.Records.Count - 1, 0);

							var summonedMonster = Globals.MDB[rl];

							Debug.Assert(summonedMonster != null);

							if (!summonedMonster.IsInRoom(characterRoom) && summonedMonster.Seen)
							{
								Globals.Out.Print("<<POOF!!>>  {0} appears!", summonedMonster.GetDecoratedName03(true, true, false, true, Globals.Buf));

								// Only reset for dead monsters

								if (summonedMonster.IsInLimbo())
								{
									summonedMonster.DmgTaken = 0;
								}

								summonedMonster.SetInRoom(characterRoom);

								Globals.Engine.CheckEnemies();
							}
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
