
// EditHintRecordManyFieldsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditHintRecordManyFieldsMenu : EditRecordManyFieldsMenu<IHint>, IEditHintRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			Globals.HintsModified = true;
		}

		public EditHintRecordManyFieldsMenu()
		{
			Title = "EDIT HINT RECORD FIELDS";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "hint";
		}
	}
}
