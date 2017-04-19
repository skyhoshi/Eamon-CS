
// SmileCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ISmileCommand))]
	public class SmileCommand : EamonRT.Game.Commands.SmileCommand, ISmileCommand
	{
		protected override void PlayerExecute()
		{
			// historical response from original

			if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0)
			{
				Globals.Out.WriteLine("{0}As you smile, the enemy attacks you!", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
