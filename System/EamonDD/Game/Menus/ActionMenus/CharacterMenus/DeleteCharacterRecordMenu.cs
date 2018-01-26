
// DeleteCharacterRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteCharacterRecordMenu : DeleteRecordMenu<ICharacter>, IDeleteCharacterRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.CharactersModified = true;
		}

		public DeleteCharacterRecordMenu()
		{
			Title = "DELETE CHARACTER RECORD";

			RecordTable = Globals.Database.CharacterTable;

			RecordTypeName = "character";
		}
	}
}
