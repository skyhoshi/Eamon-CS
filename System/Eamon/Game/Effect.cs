
// Effect.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.DataEntry;
using Eamon.Game.Extensions;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Effect : Editable, IEffect
	{
		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

		#region Interface IEffect

		public virtual string Desc { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeEffectUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IValidator

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Uid > 0;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Desc) == false && Desc.Length <= Constants.EffDescLen;
		}

		protected virtual bool ValidateInterdependenciesDesc(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Desc, args.Buf, false, false, ref invalidUid);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", invalidUid, "which doesn't exist");

				args.ErrorMessage = args.Buf.ToString();

				args.RecordType = typeof(IEffect);

				args.NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		#endregion

		#region Interface IEditable

		#region PrintFieldDesc Methods

		protected virtual void PrintDescDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter a detailed description of the effect.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		#endregion

		#region List Methods

		protected virtual void ListUid(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Uid);
			}
		}

		protected virtual void ListDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			args.Buf.Clear();

			if (args.ResolveEffects)
			{
				var rc = Globals.Engine.ResolveUidMacros(Desc, args.Buf, true, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}
			else
			{
				args.Buf.Append(Desc);
			}

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var desc = Desc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", desc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.EffDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Desc = args.Buf.ToString();      // Trim() intentionally omitted

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		#endregion

		#endregion

		#region Class Effect

		protected virtual void SetEffectUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetEffectUid();

				IsUidRecycled = true;
			}
			else if (!editRec)
			{
				IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

		public override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Uid";
						x.Validate = ValidateUid;
						x.List = ListUid;
						x.Input = InputUid;
						x.GetPrintedName = () => "Uid";
						x.GetValue = () => Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => IsUidRecycled;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Desc";
						x.Validate = ValidateDesc;
						x.ValidateInterdependencies = ValidateInterdependenciesDesc;
						x.PrintDesc = PrintDescDesc;
						x.List = ListDesc;
						x.Input = InputDesc;
						x.GetPrintedName = () => "Description";
						x.GetValue = () => Desc;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Interface IEditable

		public override void ListErrorField(IValidateArgs args)
		{
			Debug.Assert(args != null && args.ErrorField != null && args.Buf != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Uid").GetPrintedName(), null), Uid);

			Globals.Out.WriteLine("{0}{1}{0}{0}{2}{3}",
				Environment.NewLine,
				Globals.Engine.BuildPrompt(27, '.', 0, GetField("Desc").GetPrintedName(), null),
				Desc,
				string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase) ? "" : Environment.NewLine);
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IEffect effect)
		{
			return this.Uid.CompareTo(effect.Uid);
		}

		#endregion

		#region Interface IEffect

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}{0}", Environment.NewLine, Desc);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Effect

		public Effect()
		{
			SetUidIfInvalid = SetEffectUidIfInvalid;

			IsUidRecycled = true;

			Desc = "";
		}

		#endregion

		#endregion
	}
}
