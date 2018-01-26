
// EditEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditEffectRecordMenu : EditRecordManyFieldsMenu<IEffect>, IEditEffectRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.EffectsModified = true;
		}

		public EditEffectRecordMenu()
		{
			Title = "EDIT EFFECT RECORD FIELDS";

			RecordTable = Globals.Database.EffectTable;

			RecordTypeName = "effect";
		}
	}
}
