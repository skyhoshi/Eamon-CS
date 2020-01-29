
// EffectHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class EffectHelper : Helper<IEffect>, IEffectHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		// do nothing

		#endregion

		#region GetName Methods

		// do nothing

		#endregion

		#region GetValue Methods

		// do nothing

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.EffDescLen;
		}

		#endregion

		#region ValidateInterdependencies Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateInterdependenciesDesc()
		{
			var result = true;

			long invalidUid = 0;

			var rc = gEngine.ResolveUidMacros(Record.Desc, Buf, false, false, ref invalidUid);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Desc"), "effect", invalidUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		protected virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the effect.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		#endregion

		#region List Methods

		/// <summary></summary>
		protected virtual void ListUid()
		{
			if (!ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
			}
		}

		/// <summary></summary>
		protected virtual void ListDesc()
		{
			Buf.Clear();

			if (ResolveEffects)
			{
				var rc = gEngine.ResolveUidMacros(Record.Desc, Buf, true, true);

				Debug.Assert(gEngine.IsSuccess(rc));
			}
			else
			{
				Buf.Append(Record.Desc);
			}

			var listNum = NumberFields ? ListNum++ : 0;

			gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Desc"), null), Buf);
		}

		#endregion

		#region Input Methods

		/// <summary></summary>
		protected virtual void InputUid()
		{
			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputDesc()
		{
			var fieldDesc = FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", desc);

				PrintFieldDesc("Desc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Desc"), null));

				gOut.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.EffDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.Desc = Buf.ToString();      // Trim() intentionally omitted

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class EffectHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetEffectUid();

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

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.WriteLine("{0}{1}{0}{0}{2}{3}",
				Environment.NewLine,
				gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Desc"), null),
				Record.Desc,
				string.Equals(ErrorFieldName, "Desc", StringComparison.OrdinalIgnoreCase) ? "" : Environment.NewLine);
		}

		#endregion

		#region Class EffectHelper

		public EffectHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Desc"
			};
		}

		#endregion

		#endregion
	}
}
