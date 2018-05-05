
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, EamonRT.Framework.Commands.IReadCommand
	{
		protected virtual IGameState GameState { get; set; }

		protected override void PlayerProcessEvents()
		{
			// saving throw vs. intellect for book trap warning

			if (DobjArtifact.Uid == 9)
			{
				if (GameState.BookWarning == 0)
				{
					var rl = Globals.Engine.RollDice01(1, 22, 2);

					if (rl < Globals.Character.GetStats(Enums.Stat.Intellect))
					{
						Globals.Engine.PrintEffectDesc(14);

						GameState.BookWarning = 1;

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
				base.PlayerProcessEvents();
			}
		}

		protected override void PlayerProcessEvents01()
		{
			// book trap

			if (DobjArtifact.Uid == 9)
			{
				Globals.Out.Print(ActorRoom.Uid == 26 ? "You fall into the sea and are eaten by a big fish." : "You flop three times and die.");

				GameState.Die = 1;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents01();
			}
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// change name of bottle

			if (DobjArtifact.Uid == 3)
			{
				DobjArtifact.Name = "healing potion";

				Globals.Out.Print("It says, \"HEALING POTION\".");

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}

		public ReadCommand()
		{
			GameState = Globals.GameState as IGameState;

			Debug.Assert(GameState != null);
		}
	}
}
