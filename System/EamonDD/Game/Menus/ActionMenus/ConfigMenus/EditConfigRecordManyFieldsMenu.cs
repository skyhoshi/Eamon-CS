
// EditConfigRecordManyFieldsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.DataEntry;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditConfigRecordManyFieldsMenu : EditConfigRecordMenu, IEditConfigRecordManyFieldsMenu
	{
		public override void Execute()
		{
			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("EDIT CONFIG RECORD FIELDS", true);

			if (EditRecord == null)
			{
				EditRecord = Globals.Config;
			}

			var editConfig01 = Globals.CloneInstance(EditRecord);

			var editable = editConfig01 as IEditable;

			Debug.Assert(editable != null);

			editable.InputRecord(true, Globals.Config.FieldDesc);

			CompareAndSave(editConfig01);

			EditRecord = null;
		}

		public EditConfigRecordManyFieldsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
