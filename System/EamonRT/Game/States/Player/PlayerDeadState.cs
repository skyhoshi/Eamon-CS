﻿
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

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
			if (gGameState.Die > 0)
			{
				var restoreGame = false;

				var monster = gMDB[gGameState.Cm];

				Debug.Assert(monster != null);

				gEngine.DeadMenu(monster, PrintLineSep, ref restoreGame);

				if (restoreGame)
				{
					Globals.CommandParser.Clear();

					Globals.CommandParser.ActorMonster = gMDB[gGameState.Cm];

					Globals.CommandParser.InputBuf.SetFormat("restore");

					if (NextState == null)
					{
						NextState = Globals.CreateInstance<IProcessPlayerInputState>();
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
