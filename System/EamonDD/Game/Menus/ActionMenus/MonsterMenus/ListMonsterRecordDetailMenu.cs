
// ListMonsterRecordDetailMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListMonsterRecordDetailMenu : ListRecordDetailMenu<IMonster>, IListMonsterRecordDetailMenu
	{
		public ListMonsterRecordDetailMenu()
		{
			Title = "LIST MONSTER RECORD DETAILS";

			RecordTable = Globals.Database.MonsterTable;

			RecordTypeName = "monster";
		}
	}
}
