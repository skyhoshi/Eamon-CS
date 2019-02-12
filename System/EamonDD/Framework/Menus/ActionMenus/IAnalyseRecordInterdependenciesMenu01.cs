
// IAnalyseRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IAnalyseRecordInterdependenciesMenu01<out T> where T : class, IGameBase
	{
		/// <summary></summary>
		IList<string> SkipFieldNames { get; set; }

		/// <summary></summary>
		bool ClearSkipFieldNames { get; set; }

		/// <summary></summary>
		bool ModifyFlag { get; set; }

		/// <summary></summary>
		bool ExitFlag { get; set; }

		/// <summary></summary>
		void Execute();
	}
}
