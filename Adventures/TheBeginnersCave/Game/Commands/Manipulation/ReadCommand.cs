
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PpeBeforeArtifactReadTextPrint)
			{
				// saving throw vs. intellect for book trap warning

				if (DobjArtifact.Uid == 9)
				{
					if (gameState.BookWarning == 0)
					{
						var rl = Globals.Engine.RollDice(1, 22, 2);

						if (rl <= Globals.Character.GetStats(Stat.Intellect))
						{
							Globals.Engine.PrintEffectDesc(14);

							gameState.BookWarning = 1;

							GotoCleanup = true;
						}
					}
					else
					{
						Globals.Engine.PrintEffectDesc(15);
					}
				}
				else
				{
					base.PlayerProcessEvents(eventType);
				}
			}
			else if (eventType == PpeAfterArtifactRead)
			{
				// book trap

				if (DobjArtifact.Uid == 9)
				{
					Globals.Out.Print(ActorRoom.Uid == 26 ? "You fall into the sea and are eaten by a big fish." : "You flop three times and die.");

					gameState.Die = 1;

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}
				else
				{
					base.PlayerProcessEvents(eventType);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// change name of bottle

			if (DobjArtifact.Uid == 3)
			{
				DobjArtifact.Name = "healing potion";

				Globals.Out.Print("It says, \"HEALING POTION\".");

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
