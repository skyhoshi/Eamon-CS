
// Room.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(Eamon.Framework.IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override long GetDirs(long index)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			if (gameState != null)        // null in EamonDD; non-null in EamonRT
			{
				if (Uid == 33)
				{
					var oakDoorArtifact = Globals.ADB[85];

					var ac = oakDoorArtifact != null ? oakDoorArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate) : null;

					return ac != null && (ac.GetKeyUid() <= 0 || !oakDoorArtifact.Seen) && index == 1 ? 18 : base.GetDirs(index);
				}
				else if (Uid == 45)
				{
					var cellDoorArtifact = Globals.ADB[87];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate) : null;

					return ac != null && ac.GetKeyUid() <= 0 && index == 4 ? 26 : base.GetDirs(index);
				}
				else if (Uid == 46)
				{
					var cellDoorArtifact = Globals.ADB[88];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate) : null;

					return ac != null && ac.GetKeyUid() <= 0 && index == 3 ? 27 : base.GetDirs(index);
				}
				else if (Uid == 55)
				{
					var cellDoorArtifact = Globals.ADB[86];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate) : null;

					return ac != null && ac.GetKeyUid() <= 0 && index == 1 ? 56 : base.GetDirs(index);
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
