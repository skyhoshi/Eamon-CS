
// IAnalyseRecordInterdependenciesMenu01.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAnalyseRecordInterdependenciesMenu01<out T> where T : class, IHaveUid
	{
		IList<IField> SkipFields { get; set; }

		bool ClearSkipFields { get; set; }

		bool ModifyFlag { get; set; }

		bool ExitFlag { get; set; }

		void Execute();
	}
}
