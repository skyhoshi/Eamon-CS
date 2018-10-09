
// DeleteRecordMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Extensions;
using EamonDD.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class DeleteRecordMenu<T, U> : RecordMenu<T>, IDeleteRecordMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record to delete", RecordTypeName), null));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			var recordUid = Convert.ToInt64(Buf.Trim().ToString());

			Globals.Out.Print("{0}", Globals.LineSep);

			var record = RecordTable.FindRecord(recordUid);

			if (record == null)
			{
				Globals.Out.Print("{0} record not found.", RecordTypeName.FirstCharToUpper());

				goto Cleanup;
			}

			var character = record as ICharacter;

			if (character != null && character.Status != Enums.Status.Alive && character.Status != Enums.Status.Dead)
			{
				Globals.Out.Print("{0} record Status not marked as Alive or Dead.", RecordTypeName.FirstCharToUpper());

				goto Cleanup;
			}

			var helper = Globals.CreateInstance<U>(x =>
			{
				x.Record = record;
			});
			
			helper.ListRecord(true, true, false, true, false, false);

			PrintPostListLineSep();

			Globals.Thread.Sleep(150);

			Globals.Out.Write("{0}Would you like to delete this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				goto Cleanup;
			}

			record = RecordTable.RemoveRecord(recordUid);

			Debug.Assert(record != null);

			record.Dispose();

			UpdateGlobals();

		Cleanup:

			;
		}
	}
}
