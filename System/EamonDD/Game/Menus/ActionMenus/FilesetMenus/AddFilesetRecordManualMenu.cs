
// AddFilesetRecordManualMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddFilesetRecordManualMenu : AddRecordManualMenu<IFileset>, IAddFilesetRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			Globals.FilesetsModified = true;
		}

		public AddFilesetRecordManualMenu()
		{
			Title = "ADD FILESET RECORD";

			RecordTable = Globals.Database.FilesetTable;

			RecordTypeName = "fileset";
		}
	}
}
