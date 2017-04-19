
// EffectRecordUtilitiesMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class EffectRecordUtilitiesMenu : Menu, IEffectRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintEffectMenuSubtitle();
		}

		public EffectRecordUtilitiesMenu()
		{
			Title = "EFFECT RECORD UTILITIES MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			if (Globals.DdEngine.IsAdventureFilesetLoaded())
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Analyse effect record interdependencies.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IAnalyseEffectRecordInterdependenciesMenu>();
				}));
			}

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
