
// AnalyseRecordInterdependenciesMenu01.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using EamonDD.Framework.Menus.ActionMenus;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AnalyseRecordInterdependenciesMenu01<T> : IAnalyseRecordInterdependenciesMenu01<T> where T : class, IGameBase
	{
		protected virtual IAnalyseRecordInterdependenciesMenu<T> AnalyseMenu { get; set; }

		public virtual IList<IField> SkipFields
		{
			get
			{
				return AnalyseMenu.SkipFields;
			}

			set
			{
				AnalyseMenu.SkipFields = value;
			}
		}

		public virtual bool ClearSkipFields
		{
			get
			{
				return AnalyseMenu.ClearSkipFields;
			}

			set
			{
				AnalyseMenu.ClearSkipFields = value;
			}
		}

		public virtual bool ModifyFlag
		{
			get
			{
				return AnalyseMenu.ModifyFlag;
			}

			set
			{
				AnalyseMenu.ModifyFlag = value;
			}
		}

		public virtual bool ExitFlag
		{
			get
			{
				return AnalyseMenu.ExitFlag;
			}

			set
			{
				AnalyseMenu.ExitFlag = value;
			}
		}

		public virtual void Execute()
		{
			AnalyseMenu.Execute();
		}

		public AnalyseRecordInterdependenciesMenu01()
		{

		}
	}
}
