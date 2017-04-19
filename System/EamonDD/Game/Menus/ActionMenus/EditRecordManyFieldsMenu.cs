
// EditRecordManyFieldsMenu.cs

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
	public abstract class EditRecordManyFieldsMenu<T> : RecordMenu<T>, IEditRecordManyFieldsMenu<T> where T : class, IHaveUid
	{
		public virtual T EditRecord { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			if (EditRecord == null)
			{
				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record to edit", RecordTypeName), "1"));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var recordUid = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				EditRecord = RecordTable.FindRecord(recordUid);

				if (EditRecord == null)
				{
					Globals.Out.WriteLine("{0}{1} record not found.", Environment.NewLine, RecordTypeName.FirstCharToUpper());

					goto Cleanup;
				}
			}

			var editRecord01 = Globals.CloneInstance(EditRecord);

			var editable = editRecord01 as IEditable;

			Debug.Assert(editable != null);

			editable.InputRecord(true, Globals.Config.FieldDesc);

			Globals.Thread.Sleep(150);

			if (!Globals.CompareInstances(EditRecord, editRecord01))
			{
				Globals.Out.Write("{0}Would you like to save this updated {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var character = editRecord01 as ICharacter;

				if (character != null)
				{
					character.StripPoundCharsFromWeaponNames();

					character.AddPoundCharsToWeaponNames();
				}

				var artifact = editRecord01 as IArtifact;

				if (artifact != null)
				{
					rc = artifact.SyncArtifactClasses();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Engine.TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
				}
				
				var effect = editRecord01 as IEffect;

				if (effect != null)
				{
					Globals.Engine.TruncatePluralTypeEffectDesc(effect);
				}
				
				var monster = editRecord01 as IMonster;

				if (monster != null)
				{
					Globals.Engine.TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
				}

				var record = RecordTable.RemoveRecord(EditRecord.Uid);

				Debug.Assert(record != null);

				rc = RecordTable.AddRecord(editRecord01);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				UpdateGlobals();
			}
			else
			{
				Globals.Out.WriteLine("{0}{1} record not modified.", Environment.NewLine, RecordTypeName.FirstCharToUpper());
			}

		Cleanup:

			EditRecord = null;
		}
	}
}
