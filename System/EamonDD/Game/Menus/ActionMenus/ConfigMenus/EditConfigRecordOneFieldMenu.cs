
// EditConfigRecordOneFieldMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.DataEntry;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditConfigRecordOneFieldMenu : EditConfigRecordMenu, IEditConfigRecordOneFieldMenu
	{
		public virtual IField EditField { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("EDIT CONFIG RECORD FIELD", true);

			if (EditRecord == null)
			{
				EditRecord = Globals.Config;
			}

			var editConfig01 = Globals.CloneInstance(EditRecord);

			var editable = editConfig01 as IEditable;

			Debug.Assert(editable != null);

			var haveFields = editConfig01 as IHaveFields;

			Debug.Assert(haveFields != null);

			IField editField01 = null;

			if (EditField == null)
			{
				editable.ListRecord(true, true, false, true, true, true);

				Globals.Out.WriteLine();

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(47, '\0', 0, "Enter the number of the field to edit", "0"));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var fieldNum = Convert.ToInt64(Buf.Trim().ToString());

				editField01 = haveFields.GetField(fieldNum);

				if (editField01 == null)
				{
					goto Cleanup;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				editField01 = haveFields.GetField(EditField.Name);
			}

			var args = Globals.CreateInstance<IInputArgs>(x =>
			{
				x.EditRec = true;
				x.EditField = true;
				x.FieldDesc = Globals.Config.FieldDesc;
			});

			editable.InputField(editField01, args);

			CompareAndSave(editConfig01);

		Cleanup:

			EditRecord = null;

			EditField = null;
		}

		public EditConfigRecordOneFieldMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
