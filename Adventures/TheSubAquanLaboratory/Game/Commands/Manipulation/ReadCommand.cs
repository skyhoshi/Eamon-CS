
// ReadCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IReadCommand))]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		protected override void PrintCantVerbObj(Eamon.Framework.IGameBase obj)
		{
			Debug.Assert(obj != null);

			Globals.Out.Write("{0}You stare at {1}, but you don't see any secret messages forming.{0}", Environment.NewLine, obj.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected override void PlayerExecute()
		{
			RetCode rc;

			var rl = 0L;

			Debug.Assert(DobjArtifact != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			switch (DobjArtifact.Uid)
			{
				case 9:

					// Bronze plaque

					if (!gameState.ReadPlaque)
					{
						gameState.QuestValue += 250;

						gameState.ReadPlaque = true;
					}

					base.PlayerExecute();

					break;

				case 48:

					// Display screen

					rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (rl < 34)
					{
						var ls = new string[]
						{
							"",
							"SINCLAIR INLET     ",
							"VLADIVOSTOK     ",
							"BAJA PENINSULA     ",
							"YUKATAN PENINSULA     ",
							"GOLD COAST     ",
							"UPPER RHINE VALLEY     ",
							"LHASA     ",
							"VANCOUVER     ",
							"TRIPOLI     ",
							"HANOI     "
						};

						var d = new long[]
						{
							0L,
							Globals.Engine.RollDice01(1, 9, 0),
							Globals.Engine.RollDice01(1, 99, 0),
							Globals.Engine.RollDice01(1, 30, 0),
							Globals.Engine.RollDice01(1, 100, 0),
							Globals.Engine.RollDice01(1, 2, 0),
							Globals.Engine.RollDice01(1, 2, -1) + 3,
							Globals.Engine.RollDice01(1, 90, 0),
							Globals.Engine.RollDice01(1, 180, 0),
							Globals.Engine.RollDice01(1, 59, 0),
							Globals.Engine.RollDice01(1, 59, 0),
							Globals.Engine.RollDice01(1, 59, 0),
							Globals.Engine.RollDice01(1, 59, 0),
							Globals.Engine.RollDice01(1, 10, 0),
							Globals.Engine.RollDice01(1, 20, 0),
							Globals.Engine.RollDice01(1, 100, 0)
						};

						var nsd = d[5] == 1 ? "North" : "South";

						var ewd = d[6] == 3 ? "East" : "West";

						Globals.Engine.PrintEffectDesc(51);

						Globals.Out.Write("{0}{1}{2}.{3} GMT{0}", Environment.NewLine, ls[d[13]], d[14], d[15]);

						Globals.Out.Write("{0}Magnitude.....{1}.{2}", Environment.NewLine, d[1], d[2]);

						Globals.Out.Write("{0}Duration......{1}.{2} seconds", Environment.NewLine, d[3], d[4]);

						Globals.Out.Write("{0}Epicenter.....{1} Latitude {2} degrees {3} minutes {4} seconds", Environment.NewLine, nsd, d[7], d[9], d[11]);

						Globals.Out.Write("{0}{1,14}{2} Longitude {3} degrees {4} minutes {5} seconds{0}", Environment.NewLine, "", ewd, d[8], d[10], d[12]);
						
						if (!gameState.ReadDisplayScreen)
						{
							gameState.QuestValue += 300;

							gameState.ReadDisplayScreen = true;
						}
					}
					else
					{
						Globals.Out.Write("{0}The monitor screen remains blank.{0}", Environment.NewLine);
					}

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

					break;

				case 50:

					// Terminals

					rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (rl < 51)
					{
						Globals.Engine.PrintEffectDesc(52);

						if (!gameState.ReadTerminals)
						{
							gameState.QuestValue += 350;

							gameState.ReadTerminals = true;
						}
					}
					else
					{
						rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Out.Write("{0}As you watch, the terminal screen prints:{0}", Environment.NewLine);

						Globals.Out.Write("{0}  Error #{1}{0}", Environment.NewLine, rl);

						Globals.Out.Write("{0}Uploading execution impossible - attempting to abort!{0}", Environment.NewLine);
					}

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

					break;

				default:

					base.PlayerExecute();

					break;
			}
		}
	}
}
