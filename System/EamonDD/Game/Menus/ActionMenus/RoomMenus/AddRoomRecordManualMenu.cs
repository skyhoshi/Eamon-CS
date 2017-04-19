
// AddRoomRecordManualMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddRoomRecordManualMenu : AddRecordManualMenu<IRoom>, IAddRoomRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			Globals.RoomsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumRooms++;

				Globals.ModulesModified = true;
			}
		}

		public AddRoomRecordManualMenu()
		{
			Title = "ADD ROOM RECORD";

			RecordTable = Globals.Database.RoomTable;

			RecordTypeName = "room";
		}
	}
}
