
// EditRoomRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditRoomRecordManyFieldsMenu : EditRecordManyFieldsMenu<IRoom>, IEditRoomRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			Globals.RoomsModified = true;
		}

		public EditRoomRecordManyFieldsMenu()
		{
			Title = "EDIT ROOM RECORD FIELDS";

			RecordTable = Globals.Database.RoomTable;

			RecordTypeName = "room";
		}
	}
}
