
// EditCharacterRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditCharacterRecordOneFieldMenu : EditRecordOneFieldMenu<ICharacter>, IEditCharacterRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			Globals.CharactersModified = true;
		}

		public EditCharacterRecordOneFieldMenu()
		{
			Title = "EDIT CHARACTER RECORD FIELD";

			RecordTable = Globals.Database.CharacterTable;

			RecordTypeName = "character";
		}
	}
}
