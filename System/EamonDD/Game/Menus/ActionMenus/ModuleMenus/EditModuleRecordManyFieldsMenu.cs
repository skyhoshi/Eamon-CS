
// EditModuleRecordManyFieldsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.DataEntry;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditModuleRecordManyFieldsMenu : EditModuleRecordMenu, IEditModuleRecordManyFieldsMenu
	{
		public override void Execute()
		{
			if (EditRecord != null || Globals.Module != null)
			{
				Globals.Out.WriteLine();

				Globals.Engine.PrintTitle("EDIT MODULE RECORD FIELDS", true);

				if (EditRecord == null)
				{
					EditRecord = Globals.Module;
				}

				var editModule01 = Globals.CloneInstance(EditRecord);

				var editable = editModule01 as IEditable;

				Debug.Assert(editable != null);

				editable.InputRecord(true, Globals.Config.FieldDesc);

				CompareAndSave(editModule01);
			}

			EditRecord = null;
		}

		public EditModuleRecordManyFieldsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
