
// AddRecordCopyMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.DataEntry;
using Eamon.Game.Extensions;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AddRecordCopyMenu<T> : RecordMenu<T>, IAddRecordCopyMenu<T> where T : class, IHaveUid
	{
		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record to copy", RecordTypeName), "1"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			var recordUid = Convert.ToInt64(Buf.Trim().ToString());

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			var record = RecordTable.FindRecord(recordUid);

			if (record == null)
			{
				Globals.Out.WriteLine("{0}{1} record not found.", Environment.NewLine, RecordTypeName.FirstCharToUpper());

				goto Cleanup;
			}

			var record01 = Globals.CloneInstance(record);

			Debug.Assert(record01 != null);

			if (!Globals.Config.GenerateUids)
			{
				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record copy", RecordTypeName), null));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				recordUid = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				if (recordUid > 0)
				{
					record = RecordTable.FindRecord(recordUid);

					if (record != null)
					{
						Globals.Out.WriteLine("{0}{1} record already exists.", Environment.NewLine, RecordTypeName.FirstCharToUpper());

						goto Cleanup;
					}

					RecordTable.FreeUids.Remove(recordUid);

					record01.Uid = recordUid;

					record01.IsUidRecycled = false;
				}
				else
				{
					record01.Uid = RecordTable.GetRecordUid();

					record01.IsUidRecycled = true;
				}
			}
			else
			{
				record01.Uid = RecordTable.GetRecordUid();

				record01.IsUidRecycled = true;
			}

			var editable = record01 as IEditable;

			Debug.Assert(editable != null);

			editable.ListRecord(true, true, false, true, false, false);

			PrintPostListLineSep();

			Globals.Out.Write("{0}Would you like to save this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				record01.Dispose();

				goto Cleanup;
			}

			rc = RecordTable.AddRecord(record01);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			UpdateGlobals();

		Cleanup:

			;
		}
	}
}
