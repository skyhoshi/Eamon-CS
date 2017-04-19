
// MainHallMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using EamonMH.Framework.Menus.HierarchicalMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class MainHallMenu : Menu, IMainHallMenu
	{
		public override void PrintSubtitle()
		{
			Globals.MhMenu.PrintMainHallMenuSubtitle();
		}

		public override bool ShouldBreakMenuLoop()
		{
			return Globals.GoOnAdventure;
		}

		public override void Shutdown()
		{
			if (!Globals.GoOnAdventure)
			{
				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}As you leave the hall, the Irishman comes up to you, slaps you on the back and says, \"Y'all come back real soon, ya heah?\"{0}", Environment.NewLine);

				Globals.In.KeyPress(Buf);
			}
		}

		public MainHallMenu()
		{
			Title = "MAIN HALL";

			Buf = Globals.Buf;

			MenuItems = new List<IMenuItem>();

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Go on an adventure.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IGoOnAdventureMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Visit the Weapons and Armor Shop.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IMarcosCavielliMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Hire a Wizard to teach you some spells.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IHokasTokasMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Find the banker to deposit or withdraw some gold.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IShylockMcFennyMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Examine your abilities.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IExamineAbilitiesMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItems.Count);
				x.LineText = string.Format("{0}{1}. Enter the Village.", Environment.NewLine, MenuItems.Count + 1);
				x.SubMenu = Globals.CreateInstance<IVillageMenu>();
			}));

			MenuItems.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Temporarily leave the universe.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
