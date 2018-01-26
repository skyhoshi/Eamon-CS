
// IMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Text;

namespace Eamon.Framework.Menus
{
	public interface IMenu
	{
		string Title { get; set; }

		StringBuilder Buf { get; set; }

		IList<IMenuItem> MenuItems { get; set; }

		bool IsCharMenuItem(char ch);

		void PrintSubtitle();

		bool ShouldBreakMenuLoop();

		void Startup();

		void Shutdown();

		void Execute();
	}
}
