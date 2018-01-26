
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IInventoryCommand))]
	public class InventoryCommand : EamonRT.Game.Commands.InventoryCommand, IInventoryCommand
	{
		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override bool IsAllowedInRoom()
		{
			return DobjArtifact == null || Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}
	}
}
