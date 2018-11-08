
// ConfigHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class ConfigHelper : Helper<IConfig>, IConfigHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		protected virtual string GetPrintedNameShowDesc()
		{
			return "Show Descs";
		}

		protected virtual string GetPrintedNameResolveEffects()
		{
			return "Resolve Effects";
		}

		protected virtual string GetPrintedNameGenerateUids()
		{
			return "Generate Uids";
		}

		protected virtual string GetPrintedNameFieldDesc()
		{
			return "Field Descs";
		}

		#endregion

		#region GetName Methods

		// do nothing

		#endregion

		#region GetValue Methods

		// do nothing

		#endregion

		#region Validate Methods

		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateFieldDesc()
		{
			return Enum.IsDefined(typeof(Enums.FieldDesc), Record.FieldDesc);
		}

		protected virtual bool ValidateWordWrapMargin()
		{
			return Record.WordWrapMargin == Constants.RightMargin;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		protected virtual void PrintDescShowDesc()
		{
			var fullDesc = "Enter whether to omit or show descriptions during record detail listing.";

			var briefDesc = string.Format("{0}=Omit descriptions; {1}=Show descriptions", Convert.ToInt64(false), Convert.ToInt64(true));

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescResolveEffects()
		{
			var fullDesc = "Enter whether to show or resolve effect uids in descriptions during record detail listing.";

			var briefDesc = string.Format("{0}=Show effect uids; {1}=Resolve effect uids", Convert.ToInt64(false), Convert.ToInt64(true));

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescGenerateUids()
		{
			var fullDesc = "Enter whether to allow user input of uids or use system generated uids when adding new records.";

			var briefDesc = string.Format("{0}=Allow user input of uids; {1}=Use system generated uids", Convert.ToInt64(false), Convert.ToInt64(true));

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescFieldDesc()
		{
			var fullDesc = "Enter the verbosity of the field descriptions shown during record input.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var fieldDescValues = EnumUtil.GetValues<Enums.FieldDesc>();

			for (var j = 0; j < fieldDescValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)fieldDescValues[j], Globals.Engine.GetFieldDescNames(fieldDescValues[j]));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		#endregion

		#region List Methods

		protected virtual void ListUid()
		{
			if (!ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
			}
		}

		protected virtual void ListShowDesc()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ShowDesc"), null), Convert.ToInt64(Record.ShowDesc));
		}

		protected virtual void ListResolveEffects()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ResolveEffects"), null), Convert.ToInt64(Record.ResolveEffects));
		}

		protected virtual void ListGenerateUids()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("GenerateUids"), null), Convert.ToInt64(Record.GenerateUids));
		}

		protected virtual void ListFieldDesc()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("FieldDesc"), null), Globals.Engine.GetFieldDescNames(Record.FieldDesc));
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputShowDesc()
		{
			var fieldDesc = FieldDesc;

			var showDesc = Record.ShowDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(showDesc));

				PrintFieldDesc("ShowDesc", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ShowDesc"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ShowDesc = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("ShowDesc"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputResolveEffects()
		{
			var fieldDesc = FieldDesc;

			var resolveEffects = Record.ResolveEffects;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(resolveEffects));

				PrintFieldDesc("ResolveEffects", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ResolveEffects"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ResolveEffects = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("ResolveEffects"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputGenerateUids()
		{
			var fieldDesc = FieldDesc;

			var generateUids = Record.GenerateUids;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(generateUids));

				PrintFieldDesc("GenerateUids", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("GenerateUids"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.GenerateUids = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("GenerateUids"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputFieldDesc()
		{
			var fieldDesc = FieldDesc;

			var fieldDesc01 = Record.FieldDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)fieldDesc01);

				PrintFieldDesc("FieldDesc", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("FieldDesc"), "2"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "2", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.FieldDesc = (Enums.FieldDesc)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("FieldDesc"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class ConfigHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetConfigUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#endregion

		#region Class ConfigHelper

		public ConfigHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"ShowDesc",
				"ResolveEffects",
				"GenerateUids",
				"FieldDesc",
				"WordWrapMargin",
				"DdFilesetFileName",
				"DdCharacterFileName",
				"DdModuleFileName",
				"DdRoomFileName",
				"DdArtifactFileName",
				"DdEffectFileName",
				"DdMonsterFileName",
				"DdHintFileName",
				"MhWorkDir",
				"MhFilesetFileName",
				"MhCharacterFileName",
				"MhEffectFileName",
				"RtFilesetFileName",
				"RtCharacterFileName",
				"RtModuleFileName",
				"RtRoomFileName",
				"RtArtifactFileName",
				"RtEffectFileName",
				"RtMonsterFileName",
				"RtHintFileName",
				"RtGameStateFileName",
				"DdEditingFilesets",
				"DdEditingCharacters",
				"DdEditingModules",
				"DdEditingRooms",
				"DdEditingArtifacts",
				"DdEditingEffects",
				"DdEditingMonsters",
				"DdEditingHints",
			};
		}

		#endregion

		#endregion
	}
}
