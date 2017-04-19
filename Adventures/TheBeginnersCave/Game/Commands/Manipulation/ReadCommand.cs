
// ReadCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using TheBeginnersCave.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IReadCommand))]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		protected virtual IGameState GameState { get; set; }

		protected override void PlayerProcessEvents()
		{
			// saving throw vs. intellect for book trap warning

			if (DobjArtifact.Uid == 9)
			{
				if (GameState.BookWarning == 0)
				{
					var rl = 0L;

					var rc = Globals.Engine.RollDice(1, 22, 2, ref rl);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

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
				Globals.Out.WriteLine(ActorRoom.Uid == 26 ?
					"{0}You fall into the sea and are eaten by a big fish." :
					"{0}You flop three times and die.",
					Environment.NewLine);

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

				Globals.Out.WriteLine("{0}It says, \"HEALING POTION\".", Environment.NewLine);

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
