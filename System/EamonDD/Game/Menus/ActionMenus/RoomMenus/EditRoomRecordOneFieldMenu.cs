
// EditRoomRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditRoomRecordOneFieldMenu : EditRecordOneFieldMenu<IRoom>, IEditRoomRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			Globals.RoomsModified = true;
		}

		public EditRoomRecordOneFieldMenu()
		{
			Title = "EDIT ROOM RECORD FIELD";

			RecordTable = Globals.Database.RoomTable;

			RecordTypeName = "room";
		}
	}
}
