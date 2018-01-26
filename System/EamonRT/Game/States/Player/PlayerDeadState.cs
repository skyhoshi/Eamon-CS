
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PlayerDeadState : State, IPlayerDeadState
	{
		public virtual bool PrintLineSep { get; set; }

		public override void Execute()
		{
			if (Globals.GameState.Die > 0)
			{
				var restoreGame = false;

				var monster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(monster != null);

				Globals.Engine.DeadMenu(monster, PrintLineSep, ref restoreGame);

				if (restoreGame)
				{
					Globals.CommandParser.Clear();

					Globals.CommandParser.ActorMonster = Globals.MDB[Globals.GameState.Cm];

					Globals.CommandParser.InputBuf.SetFormat("restore");

					if (NextState == null)
					{
						NextState = Globals.CreateInstance<IProcessPlayerInputState>(x =>
						{
							x.IncrementCurrTurn = false;
						});
					}
				}
				else
				{
					if (NextState == null)
					{
						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public PlayerDeadState()
		{
			Name = "PlayerDeadState";
		}
	}
}
