
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void ProcessEvents01()
		{
			// Exit gate

			if (Globals.GameState.R2 == -2)
			{
				Globals.Out.Print("The path is washed out!");
			}

			// Death by spooky water! / Death by moss!

			else if (Globals.GameState.R2 == -33 || Globals.GameState.R2 == -35)
			{
				var effectUid = Globals.GameState.R2 == -33 ? 3 : 4;

				Globals.Engine.PrintEffectDesc(effectUid);

				Globals.GameState.Ro = 1;

				Globals.GameState.R2 = Globals.GameState.Ro;

				NextState = Globals.CreateInstance<IAfterPlayerMoveState>();
			}

			// Don't step on any beavers!

			else if (Globals.GameState.R2 == -34)
			{
				Globals.GameState.R2 = 33;

				Globals.Engine.PrintEffectDesc(1);

				NextState = Globals.CreateInstance<IAfterPlayerMoveState>();
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
