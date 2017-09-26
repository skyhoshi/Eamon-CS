
// IAnalyseAdventureRecordTreeMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAnalyseAdventureRecordTreeMenu : IMenu
	{
		IList<string> RecordTreeStrings { get; set; }
	}
}
