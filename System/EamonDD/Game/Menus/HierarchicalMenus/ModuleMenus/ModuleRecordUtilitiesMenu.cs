
// ModuleRecordUtilitiesMenu.cs

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
	public class ModuleRecordUtilitiesMenu : Menu, IModuleRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			if (Globals.DdEngine.IsAdventureFilesetLoaded())
			{
				Globals.Out.WriteLine("{0}Editing: {1}",
					Environment.NewLine,
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.WriteLine("{0}Modules: 1", Environment.NewLine);
		}

		public ModuleRecordUtilitiesMenu()
		{
			Title = "MODULE RECORD UTILITIES MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			if (Globals.DdEngine.IsAdventureFilesetLoaded())
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Analyse module record interdependencies.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IAnalyseModuleRecordInterdependenciesMenu>();
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
