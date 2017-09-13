
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPlayerMoveCheckState))]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void ProcessEvents01()
		{
			if (Globals.GameState.R2 == -33)
			{
				Globals.Out.WriteLine("{0}The oak door is locked from the inside!", Environment.NewLine);
			}
			else if (Globals.GameState.R2 == -55)
			{
				Globals.Out.WriteLine("{0}The cell door is locked from the outside!", Environment.NewLine);
			}

			// Down the sewage chute

			else if (Globals.GameState.R2 == -60)
			{
				Globals.Engine.PrintEffectDesc(24);

				Globals.GameState.Die = 1;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});

				GotoCleanup = true;
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
