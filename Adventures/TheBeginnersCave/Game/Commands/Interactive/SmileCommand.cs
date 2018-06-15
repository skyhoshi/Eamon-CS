
// SmileCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class SmileCommand : EamonRT.Game.Commands.SmileCommand, ISmileCommand
	{
		public override void PlayerExecute()
		{
			// historical response from original

			if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0)
			{
				Globals.Out.Print("As you smile, the enemy attacks you!");

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
