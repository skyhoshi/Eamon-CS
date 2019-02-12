
// IAnalyseAdventureRecordTreeMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IAnalyseAdventureRecordTreeMenu : IMenu
	{
		/// <summary></summary>
		IList<string> RecordTreeStrings { get; set; }
	}
}
