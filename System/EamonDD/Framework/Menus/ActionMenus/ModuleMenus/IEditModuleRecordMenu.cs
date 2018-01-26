
// IEditModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IEditModuleRecordMenu : IMenu
	{
		IModule EditRecord { get; set; }
	}
}
