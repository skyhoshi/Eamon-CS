
// IMenuItem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Menus
{
	public interface IMenuItem
	{
		char SelectChar { get; set; }

		string LineText { get; set; }

		IMenu SubMenu { get; set; }
	}
}
