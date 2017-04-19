
// DeleteModuleRecordMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteModuleRecordMenu : Menu, IDeleteModuleRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			if (Globals.Module != null)
			{
				Globals.Out.WriteLine();

				Globals.Engine.PrintTitle("DELETE MODULE RECORD", true);

				Globals.Module.ListRecord(true, true, false, true, false, false);

				Globals.Out.WriteLine();

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Would you like to delete this module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var module = Globals.Database.RemoveModule(Globals.Module.Uid);

				Debug.Assert(module != null);

				module.Dispose();

				Globals.ModulesModified = true;

				Globals.Module = null;
			}

		Cleanup:

			;
		}

		public DeleteModuleRecordMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
