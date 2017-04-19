
// PushCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class PushCommand : EamonRT.Game.Commands.Command, IPushCommand
	{
		protected override void PlayerExecute()
		{
			Eamon.Framework.IArtifact artifact;

			Debug.Assert(DobjArtifact != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			switch (DobjArtifact.Uid)
			{
				case 3:

					// Elevator up button

					if (gameState.GetNBTL(Enums.Friendliness.Enemy) <= 0)
					{
						if (ActorRoom.Uid != 17)
						{
							Globals.Out.Write("{0}Up button pushed.{0}", Environment.NewLine);

							var newRoom = Globals.RDB[17];

							Debug.Assert(newRoom != null);

							var effect = Globals.EDB[2];

							Debug.Assert(effect != null);

							Globals.RtEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

							NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();
						}
						else
						{
							Globals.Engine.PrintEffectDesc(16);
						}
					}
					else
					{
						PrintEnemiesNearby();

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
					}

					goto Cleanup;

				case 4:

					// Elevator down button

					if (gameState.GetNBTL(Enums.Friendliness.Enemy) <= 0)
					{
						if (ActorRoom.Uid != 18)
						{
							Globals.Out.Write("{0}Down button pushed.{0}", Environment.NewLine);

							var newRoom = Globals.RDB[18];

							Debug.Assert(newRoom != null);

							var effect = Globals.EDB[3];

							Debug.Assert(effect != null);

							Globals.RtEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

							NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();
						}
						else
						{
							Globals.Engine.PrintEffectDesc(17);
						}
					}
					else
					{
						PrintEnemiesNearby();

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
					}

					goto Cleanup;

				case 19:
				case 20:
				case 21:

					// Black/Yellow/Red buttons (Make mystery food)

					Globals.Out.Write("{0}{1} pushed.{0}", Environment.NewLine, DobjArtifact.GetDecoratedName01(true, false, false, false, Globals.Buf));

					artifact = Globals.ADB[22];

					Debug.Assert(artifact != null);

					if (!artifact.IsInLimbo() || ++gameState.FoodButtonPushes < 3)
					{
						Globals.Engine.PrintEffectDesc(26);

						goto Cleanup;
					}

					gameState.FoodButtonPushes = 0;

					Globals.Engine.PrintEffectDesc(27);

					artifact.SetInRoom(ActorRoom);

					artifact.Seen = false;

					artifact.GetClasses(0).Field6 = 5;

					var gruel = new string[] { "",
						"piping hot", "warm", "tepid", "cool", "ice cold", "frozen",
						"red", "yellow", "blue", "purple", "orange", "green",
						"watery", "smooth and thick", "chunky, soup-like", "solid", "flaky", "granular",
						"pours", "pours", "plops", "thumps", "pours", "pours"
					};

					var d = new long[4];

					var rc = Globals.Engine.RollDice(d.Length, 6, ref d);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (d[1] > 5 && d[2] < 4)
					{
						d[2] += 3;
					}

					Globals.Out.Write("{0}{1} {2} {3} {4} substance {5} into the deposit area at the bottom of the machine.{0}", 
						Environment.NewLine,
						d[1] == 5 ? "An" : "A",
						gruel[d[1]],
						gruel[d[3] + 6],
						gruel[d[2] + 12],
						gruel[d[2] + 18]);

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					goto Cleanup;

				case 27:
				case 46:
				case 55:
				case 56:
				case 59:
				case 60:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:

					Globals.Out.Write("{0}{1} pushed.{0}", Environment.NewLine, DobjArtifact.GetDecoratedName01(true, false, false, false, Globals.Buf));

					switch (DobjArtifact.Uid)
					{
						case 27:

							// Sterilize

							artifact = Globals.ADB[84];

							Debug.Assert(artifact != null);

							if (!artifact.IsInLimbo() && !gameState.Sterilize)
							{
								Globals.Engine.PrintEffectDesc(6);

								gameState.Sterilize = true;
							}

							goto Cleanup;

						case 46:

							// Nada

							Globals.Engine.PrintEffectDesc(7);

							goto Cleanup;

						case 55:

							// Flood in

							if (gameState.Flood != 1)
							{
								Globals.Engine.PrintEffectDesc(8);

								gameState.Flood = 1;
							}

							goto Cleanup;

						case 56:

							// Flood out

							if (gameState.Flood == 1 /* && gameState.FloodLevel > 1 */)
							{
								Globals.Engine.PrintEffectDesc(9);

								gameState.Flood = 2;
							}

							goto Cleanup;

						case 66:

							// Turret up

							if (gameState.Elevation < 4)
							{
								Globals.Engine.PrintEffectDesc(10);

								gameState.Elevation++;
							}
							else
							{
								Globals.Engine.PrintEffectDesc(28);
							}

							goto Cleanup;

						case 67:

							// Turret down

							if (gameState.Elevation > 0)
							{
								Globals.Engine.PrintEffectDesc(11);

								gameState.Elevation--;
							}
							else
							{
								Globals.Engine.PrintEffectDesc(29);
							}

							goto Cleanup;

						case 68:

							// Turret rotate

							Globals.Engine.PrintEffectDesc(12);

							goto Cleanup;

						case 69:

							// Energize

							if (!gameState.Energize)
							{
								Globals.Engine.PrintEffectDesc(13);

								gameState.Energize = true;
							}
							else
							{
								Globals.Engine.PrintEffectDesc(30);
							}

							goto Cleanup;

						case 70:

							// Blue laser

							if (gameState.Energize)
							{
								Globals.Engine.PrintEffectDesc(14);

								gameState.Energize = false;
							}
							else
							{
								Globals.Engine.PrintEffectDesc(30);
							}

							goto Cleanup;

						default:

							goto Cleanup;
					}

				default:

					PrintCantVerbObj(DobjArtifact);

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public PushCommand()
		{
			SortOrder = 440;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "PushCommand";

			Verb = "push";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
