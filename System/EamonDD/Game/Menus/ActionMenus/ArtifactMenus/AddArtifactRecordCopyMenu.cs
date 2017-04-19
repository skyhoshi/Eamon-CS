
// AddArtifactRecordCopyMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddArtifactRecordCopyMenu : AddRecordCopyMenu<IArtifact>, IAddArtifactRecordCopyMenu
	{
		public override void UpdateGlobals()
		{
			Globals.ArtifactsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumArtifacts++;

				Globals.ModulesModified = true;
			}
		}

		public AddArtifactRecordCopyMenu()
		{
			Title = "COPY ARTIFACT RECORD";

			RecordTable = Globals.Database.ArtifactTable;

			RecordTypeName = "artifact";
		}
	}
}
