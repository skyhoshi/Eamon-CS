
// IAnalyseRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAnalyseRecordInterdependenciesMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		IList<string> SkipNames { get; set; }

		T ErrorRecord { get; set; }

		bool ClearSkipNames { get; set; }

		bool ModifyFlag { get; set; }

		bool ExitFlag { get; set; }

		void ProcessInterdependency();
	}
}
