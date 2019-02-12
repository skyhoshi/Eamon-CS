
// IEditRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IEditRecordManyFieldsMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		/// <summary></summary>
		T EditRecord { get; set; }
	}
}
