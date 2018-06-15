
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class InventoryCommand : EamonRT.Game.Commands.InventoryCommand, IInventoryCommand
	{
		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override bool IsAllowedInRoom()
		{
			return DobjArtifact == null || Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}
	}
}
