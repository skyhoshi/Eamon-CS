
// AnalyseHintRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseHintRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<IHint>, IAnalyseHintRecordInterdependenciesMenu
	{
		public AnalyseHintRecordInterdependenciesMenu()
		{
			Title = "ANALYSE HINT RECORDS";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "hint";
		}
	}
}
