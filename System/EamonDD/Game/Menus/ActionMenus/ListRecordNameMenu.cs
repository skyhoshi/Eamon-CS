
// ListRecordNameMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.DataEntry;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class ListRecordNameMenu<T> : RecordMenu<T>, IListRecordNameMenu<T> where T : class, IHaveUid
	{
		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			var j = RecordTable.GetRecordsCount();

			var i = 0;

			foreach (var record in RecordTable.Records)
			{
				var editable = record as IEditable;

				Debug.Assert(editable != null);

				editable.ListRecord(false, false, false, false, false, false);

				nlFlag = true;

				if ((i != 0 && (i % (Constants.NumRows - 8)) == 0) || i == j - 1)
				{
					nlFlag = false;

					PrintPostListLineSep();

					Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					if (Buf.Length > 0 && Buf[0] == 'X')
					{
						break;
					}
				}

				i++;
			}

			if (nlFlag)
			{
				Globals.Out.WriteLine();
			}

			Globals.Out.WriteLine("{0}Done listing {1} record names.", Environment.NewLine, RecordTypeName);
		}
	}
}
