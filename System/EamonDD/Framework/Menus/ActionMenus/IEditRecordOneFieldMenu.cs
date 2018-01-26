
// IEditRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IEditRecordOneFieldMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		T EditRecord { get; set; }

		string EditFieldName { get; set; }
	}
}
