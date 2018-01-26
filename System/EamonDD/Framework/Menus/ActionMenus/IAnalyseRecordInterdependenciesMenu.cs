
// IAnalyseRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAnalyseRecordInterdependenciesMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		IList<string> SkipFieldNames { get; set; }

		IValidateArgs ValidateArgs { get; set; }

		T ErrorRecord { get; set; }

		bool ClearSkipFieldNames { get; set; }

		bool ModifyFlag { get; set; }

		bool ExitFlag { get; set; }

		void ProcessInterdependency();
	}
}
