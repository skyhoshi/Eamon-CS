
// DeleteFilesetRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteFilesetRecordMenu : DeleteRecordMenu<IFileset>, IDeleteFilesetRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.FilesetsModified = true;
		}

		public DeleteFilesetRecordMenu()
		{
			Title = "DELETE FILESET RECORD";

			RecordTable = Globals.Database.FilesetTable;

			RecordTypeName = "fileset";
		}
	}
}
