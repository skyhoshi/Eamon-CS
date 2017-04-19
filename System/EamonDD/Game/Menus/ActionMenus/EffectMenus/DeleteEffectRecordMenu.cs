
// DeleteEffectRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteEffectRecordMenu : DeleteRecordMenu<IEffect>, IDeleteEffectRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		public override void UpdateGlobals()
		{
			Globals.EffectsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumEffects--;

				Globals.ModulesModified = true;
			}
		}

		public DeleteEffectRecordMenu()
		{
			Title = "DELETE EFFECT RECORD";

			RecordTable = Globals.Database.EffectTable;

			RecordTypeName = "effect";
		}
	}
}
