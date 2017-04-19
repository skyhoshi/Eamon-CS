
// EffectRecordMenu.cs

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
	public class EffectRecordMenu : Menu, IEffectRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintEffectMenuSubtitle();
		}

		public EffectRecordMenu()
		{
			Title = "EFFECT RECORD MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Add an effect record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddEffectRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Edit an effect record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditEffectRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Delete an effect record.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IDeleteEffectRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. List effect records.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IListEffectRecordMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Effect record utilities.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEffectRecordUtilitiesMenu>();
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
