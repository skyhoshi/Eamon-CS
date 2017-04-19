
// MainMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class MainMenu : Menu, IMainMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintMainMenuSubtitle();
		}

		public MainMenu()
		{
			Title = "MAIN MENU";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Config record maintenance.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IConfigRecordMenu>();
			}));

			if (Globals.Config.DdEditingFilesets)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Fileset record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IFilesetRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingCharacters)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Character record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<ICharacterRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingModules)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Module record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IModuleRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingRooms)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Room record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IRoomRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingArtifacts)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Artifact record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IArtifactRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingEffects)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Effect record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IEffectRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingMonsters)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Monster record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IMonsterRecordMenu>();
				}));
			}

			if (Globals.Config.DdEditingHints)
			{
				MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItems.Count);
					x.LineText = string.Format("{0}{1}. Hint record maintenance.", Environment.NewLine, MenuItems.Count + 1);
					x.SubMenu = Globals.CreateInstance<IHintRecordMenu>();
				}));
			}

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = MenuItems.Count + 1 > 9 ? 'U' : (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Utilities.", Environment.NewLine, MenuItems.Count + 1 > 9 ? "U" : (MenuItems.Count + 1).ToString());
				x.SubMenu = Globals.CreateInstance<IUtilitiesMenu>();
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
