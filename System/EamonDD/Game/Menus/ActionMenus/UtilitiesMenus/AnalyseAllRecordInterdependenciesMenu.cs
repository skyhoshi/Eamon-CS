
// AnalyseAllRecordInterdependenciesMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseAllRecordInterdependenciesMenu : Menu, IAnalyseAllRecordInterdependenciesMenu
	{
		public virtual IAnalyseRecordInterdependenciesMenu01<IHaveUid>[] AnalyseMenus { get; set; }

		public virtual IList<IField> SkipFields { get; set; }

		public virtual bool ModifyFlag { get; set; }

		public virtual bool ExitFlag { get; set; }

		public override void Execute()
		{
			RetCode rc;

			SkipFields.Clear();

			ExitFlag = false;

			while (true)
			{
				ModifyFlag = false;

				foreach (var menu in AnalyseMenus)
				{
					menu.Execute();

					if (menu.ExitFlag)
					{
						ExitFlag = true;
					}

					if (!ExitFlag)
					{
						if (menu.ModifyFlag)
						{
							ModifyFlag = true;
						}

						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							ExitFlag = true;
						}
					}

					if (ExitFlag)
					{
						goto ExitLoop;
					}
				}

				if (!ModifyFlag)
				{
					goto ExitLoop;
				}
			}

		ExitLoop:

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.WriteLine("{0}Done analysing all record interdependencies.", Environment.NewLine);
		}

		public AnalyseAllRecordInterdependenciesMenu()
		{
			Buf = Globals.Buf;

			SkipFields = new List<IField>();

			AnalyseMenus = new IAnalyseRecordInterdependenciesMenu01<IHaveUid>[]
			{
				Globals.CreateInstance<IAnalyseArtifactRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFields = SkipFields;
					x.ClearSkipFields = false;
				}),
				Globals.CreateInstance<IAnalyseEffectRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFields = SkipFields;
					x.ClearSkipFields = false;
				}),
				Globals.CreateInstance<IAnalyseHintRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFields = SkipFields;
					x.ClearSkipFields = false;
				}),
				Globals.CreateInstance<IAnalyseModuleRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFields = SkipFields;
					x.ClearSkipFields = false;
				}),
				Globals.CreateInstance<IAnalyseMonsterRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFields = SkipFields;
					x.ClearSkipFields = false;
				}),
				Globals.CreateInstance<IAnalyseRoomRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFields = SkipFields;
					x.ClearSkipFields = false;
				})
			};

		}
	}
}
