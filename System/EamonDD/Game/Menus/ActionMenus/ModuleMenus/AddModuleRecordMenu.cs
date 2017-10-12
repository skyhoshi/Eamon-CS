
// AddModuleRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddModuleRecordMenu : AddRecordManualMenu<IModule>, IAddModuleRecordMenu
	{
		public override void Execute()
		{
			IModule module;
			RetCode rc;

			if (Globals.Module == null)
			{
				Globals.Out.WriteLine();

				Globals.Engine.PrintTitle("ADD MODULE RECORD", true);

				if (!Globals.Config.GenerateUids && NewRecordUid == 0)
				{
					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, "Enter the uid of the module record to add", null));

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					NewRecordUid = Convert.ToInt64(Buf.Trim().ToString());

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					if (NewRecordUid > 0)
					{
						module = Globals.MODDB[NewRecordUid];

						if (module != null)
						{
							Globals.Out.WriteLine("{0}Module record already exists.", Environment.NewLine);

							goto Cleanup;
						}

						Globals.Database.ModuleTable.FreeUids.Remove(NewRecordUid);
					}
				}

				module = Globals.CreateInstance<IModule>(x =>
				{
					x.Uid = NewRecordUid;
				});

				var helper = Globals.CreateInstance<IHelper<IModule>>(x =>
				{
					x.Record = module;
				});

				helper.InputRecord(false, Globals.Config.FieldDesc);

				Globals.Thread.Sleep(150);

				Globals.Out.Write("{0}Would you like to save this module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					module.Dispose();

					goto Cleanup;
				}

				rc = Globals.Database.AddModule(module);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.ModulesModified = true;

				Globals.Module = module;
			}

		Cleanup:

			NewRecordUid = 0;
		}
	}
}
