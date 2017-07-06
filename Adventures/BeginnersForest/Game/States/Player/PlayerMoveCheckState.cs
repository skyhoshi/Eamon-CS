
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using BeginnersForest.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPlayerMoveCheckState))]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void ProcessEvents01()
		{
			// Exit gate

			if (Globals.GameState.R2 == -2)
			{
				Globals.Out.WriteLine("{0}The path is washed out!", Environment.NewLine);
			}

			// Death by spooky water! / Death by moss!

			else if (Globals.GameState.R2 == -33 || Globals.GameState.R2 == -35)
			{
				var effectUid = Globals.GameState.R2 == -33 ? 3 : 4;

				Globals.Engine.PrintEffectDesc(effectUid);

				Globals.GameState.Ro = 1;

				Globals.GameState.R2 = Globals.GameState.Ro;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();
			}

			// Don't step on any beavers!

			else if (Globals.GameState.R2 == -34)
			{
				Globals.GameState.R2 = 33;

				Globals.Engine.PrintEffectDesc(1);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
