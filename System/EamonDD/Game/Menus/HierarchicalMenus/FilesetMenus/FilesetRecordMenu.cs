
// FilesetRecordMenu.cs

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
	public class FilesetRecordMenu : Menu, IFilesetRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintFilesetMenuSubtitle();
		}

		public FilesetRecordMenu()
		{
			Title = "FILESET RECORD MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Add a fileset record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddFilesetRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Edit a fileset record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditFilesetRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Delete a fileset record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IDeleteFilesetRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. List fileset records.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IListFilesetRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Fileset record utilities.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IFilesetRecordUtilitiesMenu>();
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
