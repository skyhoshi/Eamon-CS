
// EditHintRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditHintRecordOneFieldMenu : EditRecordOneFieldMenu<IHint>, IEditHintRecordOneFieldMenu
	{
		public override void PrintPostListLineSep()
		{
			Globals.Out.Print("{0}", Globals.LineSep);
		}

		public override void UpdateGlobals()
		{
			Globals.HintsModified = true;
		}

		public EditHintRecordOneFieldMenu()
		{
			Title = "EDIT HINT RECORD FIELD";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "hint";
		}
	}
}
