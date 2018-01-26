
// IAddRecordManualMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAddRecordManualMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		long NewRecordUid { get; set; }
	}
}
