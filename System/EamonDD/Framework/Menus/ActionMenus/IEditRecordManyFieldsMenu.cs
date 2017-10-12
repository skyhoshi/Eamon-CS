
// IEditRecordManyFieldsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IEditRecordManyFieldsMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		T EditRecord { get; set; }
	}
}
