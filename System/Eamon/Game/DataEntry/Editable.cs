
// Editable.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Validation;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataEntry
{
	public abstract class Editable : Validator
	{
		#region Protected Properties

		[ExcludeFromSerialization]
		protected virtual Action<bool> SetUidIfInvalid { get; set; }

		#endregion

		#region Public Methods

		#region Interface IEditable

		public virtual void PrintFieldDesc(IField field, bool editRec, bool editField, Enums.FieldDesc fieldDesc)
		{
			if (field == null || !Enum.IsDefined(typeof(Enums.FieldDesc), fieldDesc))
			{
				// PrintError

				goto Cleanup;
			}

			if (field.PrintDesc != null)
			{
				PrintFieldDesc(field, Globals.CreateInstance<IPrintDescArgs>(x =>
				{
					x.EditRec = editRec;
					x.EditField = editField;
					x.FieldDesc = fieldDesc;
				}));
			}

		Cleanup:

			;
		}

		public virtual void PrintFieldDesc(IField field, IPrintDescArgs args)
		{
			if (field == null || args == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (field.PrintDesc != null)
			{
				field.PrintDesc(field, args);

				if (args.Buf.Length > 0)
				{
					Globals.Out.Write("{0}", args.Buf);
				}
			}

		Cleanup:

			;
		}

		public virtual void ListRecord(bool fullDetail, bool showDesc, bool resolveEffects, bool lookupMsg, bool numberFields, bool excludeROFields)
		{
			ListRecord(Globals.CreateInstance<IListArgs>(x =>
			{
				x.FullDetail = fullDetail;
				x.ShowDesc = showDesc;
				x.ResolveEffects = resolveEffects;
				x.LookupMsg = lookupMsg;
				x.NumberFields = numberFields;
				x.ExcludeROFields = excludeROFields;
			}));
		}

		public virtual void ListRecord(IListArgs args)
		{
			if (args == null)
			{
				// PrintError

				goto Cleanup;
			}

			var fields = GetFields();

			foreach (var f in fields)
			{
				ListField(f, args);
			}

		Cleanup:

			;
		}

		public virtual void ListField(IField field, IListArgs args)
		{
			if (field == null || args == null)
			{
				// PrintError

				goto Cleanup;
			}

			field.ListNum = 0;

			if (field.List != null)
			{
				field.List(field, args);
			}

		Cleanup:

			;
		}

		public virtual void ListErrorField(IValidateArgs args)
		{

		}

		public virtual void InputRecord(bool editRec, Enums.FieldDesc fieldDesc)
		{
			if (!Enum.IsDefined(typeof(Enums.FieldDesc), fieldDesc))
			{
				// PrintError

				goto Cleanup;
			}

			InputRecord(Globals.CreateInstance<IInputArgs>(x =>
			{
				x.EditRec = editRec;
				x.FieldDesc = fieldDesc;
			}));

		Cleanup:

			;
		}

		public virtual void InputRecord(IInputArgs args)
		{
			if (args == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (SetUidIfInvalid != null)
			{
				SetUidIfInvalid(args.EditRec);
			}

			var fields = GetFields();

			foreach (var f in fields)
			{
				InputField(f, args);
			}

		Cleanup:

			;
		}

		public virtual void InputField(IField field, IInputArgs args)
		{
			if (field == null || args == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (field.Input != null)
			{
				field.Input(field, args);
			}

		Cleanup:

			;
		}

		#endregion

		#endregion
	}
}
