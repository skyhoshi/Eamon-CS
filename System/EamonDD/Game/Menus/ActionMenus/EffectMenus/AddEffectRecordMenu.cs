
// AddEffectRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddEffectRecordMenu : AddRecordManualMenu<IEffect>, IAddEffectRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.EffectsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumEffects++;

				Globals.ModulesModified = true;
			}
		}

		public AddEffectRecordMenu()
		{
			Title = "ADD EFFECT RECORD";

			RecordTable = Globals.Database.EffectTable;

			RecordTypeName = "effect";
		}
	}
}
