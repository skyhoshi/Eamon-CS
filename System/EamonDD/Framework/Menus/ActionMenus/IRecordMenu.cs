
// IRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IRecordMenu<T> : IMenu where T : class, IGameBase
	{
		IDbTable<T> RecordTable { get; set; }

		string RecordTypeName { get; set; }

		void PrintPostListLineSep();

		void UpdateGlobals();
	}
}
