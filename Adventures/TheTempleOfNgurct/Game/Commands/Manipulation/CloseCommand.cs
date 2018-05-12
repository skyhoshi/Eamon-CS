
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public CloseCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
