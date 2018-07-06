
// ListModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListModuleRecordMenu : Menu, IListModuleRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			if (Globals.Module != null)
			{
				Globals.Out.WriteLine();

				Globals.Engine.PrintTitle("LIST MODULE RECORD DETAILS", true);
				
				var helper = Globals.CreateInstance<IHelper<IModule>>(x =>
				{
					x.Record = Globals.Module;
				});
				
				helper.ListRecord(true, Globals.Config.ShowDesc, Globals.Config.ResolveEffects, true, false, false);

				Globals.Out.WriteLine();

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					// do nothing
				}

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("Done listing module record details.");
			}
		}

		public ListModuleRecordMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
