
// EditModuleRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class EditModuleRecordMenu : Menu, Framework.Menus.HierarchicalMenus.IEditModuleRecordMenu
	{
		public override void PrintSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.WriteLine("{0}Editing: {1}",
					Environment.NewLine,
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.WriteLine("{0}Modules: 1", Environment.NewLine);
		}

		public EditModuleRecordMenu()
		{
			Title = "EDIT MODULE RECORD MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Edit many fields of a module record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditModuleRecordManyFieldsMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Edit one field of a module record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditModuleRecordOneFieldMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
