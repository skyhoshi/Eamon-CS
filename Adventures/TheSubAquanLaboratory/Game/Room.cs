
// Room.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override long GetDirs(long index)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			if (gameState != null)        // null in EamonDD; non-null in EamonRT
			{
				if (Uid == 2)
				{
					var backWallArtifact = Globals.ADB[83];

					return backWallArtifact != null && backWallArtifact.IsInLimbo() && index == 1 ? 17 : base.GetDirs(index);
				}
				else if (Uid == 10)
				{
					return gameState.Flood != 0 && (index == 5 || index == 7 || index == 8) ? -20 : base.GetDirs(index);
				}
				else if (Uid == 43)
				{
					var ovalDoorArtifact = Globals.ADB[16];

					return ovalDoorArtifact != null && ovalDoorArtifact.IsInLimbo() && index == 4 ? 9 : base.GetDirs(index);
				}
				else
				{
					return base.GetDirs(index);
				}
			}
			else
			{
				return base.GetDirs(index);
			}
		}
	}
}
