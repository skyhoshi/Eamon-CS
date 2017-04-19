
// IEditConfigRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IEditConfigRecordMenu : IMenu
	{
		IConfig EditRecord { get; set; }
	}
}
