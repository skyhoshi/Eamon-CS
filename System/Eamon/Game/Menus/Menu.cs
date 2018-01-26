
// Menu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework.Menus;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Menus
{
	public abstract class Menu : IMenu
	{
		public virtual string Title { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public virtual IList<IMenuItem> MenuItems { get; set; }

		public virtual bool IsCharMenuItem(char ch)
		{
			return MenuItems != null && MenuItems.FirstOrDefault(mi => mi.SelectChar == ch) != null;
		}

		public virtual void PrintSubtitle()
		{

		}

		public virtual bool ShouldBreakMenuLoop()
		{
			return false;
		}

		public virtual void Startup()
		{

		}

		public virtual void Shutdown()
		{

		}

		public virtual void Execute()
		{
			RetCode rc;
			long i;

			Startup();

			while (true)
			{
				Globals.Out.WriteLine();

				if (! string.IsNullOrWhiteSpace(Title))
				{
					Globals.Engine.PrintTitle(Title, true);
				}

				PrintSubtitle();

				for (i = 0; i < MenuItems.Count; i++)
				{
					Globals.Out.Write("{0}", MenuItems[(int)i].LineText);
				}

				Globals.Out.Write("{0}[X]: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "X", Globals.Engine.ModifyCharToUpper, IsCharMenuItem, IsCharMenuItem);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				var menuItem = MenuItems.FirstOrDefault(mi => mi.SelectChar == Buf[0]);

				Debug.Assert(menuItem != null);

				if (menuItem.SubMenu == null)
				{
					break;
				}

				menuItem.SubMenu.Execute();

				if (ShouldBreakMenuLoop())
				{
					break;
				}
			}

			Shutdown();
		}

		public Menu()
		{

		}
	}
}
