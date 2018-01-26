
// IAnalyseRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAnalyseRecordInterdependenciesMenu01<out T> where T : class, IGameBase
	{
		IList<string> SkipFieldNames { get; set; }

		bool ClearSkipFieldNames { get; set; }

		bool ModifyFlag { get; set; }

		bool ExitFlag { get; set; }

		void Execute();
	}
}
