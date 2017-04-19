
// EditArtifactRecordOneFieldMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditArtifactRecordOneFieldMenu : EditRecordOneFieldMenu<IArtifact>, IEditArtifactRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			Globals.ArtifactsModified = true;
		}

		public EditArtifactRecordOneFieldMenu()
		{
			Title = "EDIT ARTIFACT RECORD FIELD";

			RecordTable = Globals.Database.ArtifactTable;

			RecordTypeName = "artifact";
		}
	}
}
