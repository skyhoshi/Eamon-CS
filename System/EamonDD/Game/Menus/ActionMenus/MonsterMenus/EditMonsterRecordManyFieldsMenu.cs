
// EditMonsterRecordManyFieldsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditMonsterRecordManyFieldsMenu : EditRecordManyFieldsMenu<IMonster>, IEditMonsterRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			Globals.MonstersModified = true;
		}

		public EditMonsterRecordManyFieldsMenu()
		{
			Title = "EDIT MONSTER RECORD FIELDS";

			RecordTable = Globals.Database.MonsterTable;

			RecordTypeName = "monster";
		}
	}
}
