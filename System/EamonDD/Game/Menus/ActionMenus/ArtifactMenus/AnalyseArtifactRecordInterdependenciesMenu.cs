
// AnalyseArtifactRecordInterdependenciesMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseArtifactRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<IArtifact>, IAnalyseArtifactRecordInterdependenciesMenu
	{
		public AnalyseArtifactRecordInterdependenciesMenu()
		{
			Title = "ANALYSE ARTIFACT RECORDS";

			RecordTable = Globals.Database.ArtifactTable;

			RecordTypeName = "artifact";
		}
	}
}
