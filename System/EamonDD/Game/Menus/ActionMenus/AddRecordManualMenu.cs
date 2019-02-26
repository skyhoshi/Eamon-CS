
// AddRecordManualMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AddRecordManualMenu<T, U> : RecordMenu<T>, IAddRecordManualMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public virtual long NewRecordUid { get; set; }

		public override void Execute()
		{
			RetCode rc;

			T record;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			if (!Globals.Config.GenerateUids && NewRecordUid == 0)
			{
				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(55, '\0', 0, string.Format("Enter the uid of the {0} record to add", RecordTypeName), null));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				NewRecordUid = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.Print("{0}", Globals.LineSep);

				if (NewRecordUid > 0)
				{
					record = RecordTable.FindRecord(NewRecordUid);

					if (record != null)
					{
						Globals.Out.Print("{0} record already exists.", RecordTypeName.FirstCharToUpper());

						goto Cleanup;
					}

					RecordTable.FreeUids.Remove(NewRecordUid);
				}
			}

			record = Globals.CreateInstance<T>(x =>
			{
				x.Uid = NewRecordUid;
			});
			
			var helper = Globals.CreateInstance<U>(x =>
			{
				x.Record = record;
			});
			
			helper.InputRecord(false, Globals.Config.FieldDesc);

			Globals.Thread.Sleep(150);

			Globals.Out.Write("{0}Would you like to save this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				record.Dispose();

				goto Cleanup;
			}

			var character = record as ICharacter;

			if (character != null)
			{
				character.StripPoundCharsFromWeaponNames();

				character.AddPoundCharsToWeaponNames();
			}

			var artifact = record as IArtifact;

			if (artifact != null)
			{
				var i = Globals.Engine.FindIndex(artifact.Categories, ac => ac != null && ac.Type == ArtifactType.None);
				
				if (i > 0)
				{
					rc = artifact.SetArtifactCategoryCount(i);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				rc = artifact.SyncArtifactCategories();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Engine.TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
			}

			var effect = record as IEffect;

			if (effect != null)
			{
				Globals.Engine.TruncatePluralTypeEffectDesc(effect);
			}

			var monster = record as IMonster;

			if (monster != null)
			{
				Globals.Engine.TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
			}

			rc = RecordTable.AddRecord(record);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			UpdateGlobals();

		Cleanup:

			NewRecordUid = 0;
		}
	}
}
