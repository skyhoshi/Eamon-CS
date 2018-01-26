
// Helper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers.Generic
{
	public abstract class Helper<T> : IHelper<T> where T : class, IGameBase
	{
		#region Protected Fields

		protected T _record;

		#endregion

		#region Protected Properties

		protected virtual IList<IField> Fields { get; set; }

		protected virtual IField NameField { get; set; }

		protected virtual Action<bool> SetUidIfInvalid { get; set; }

		#endregion

		#region Public Properties

		public virtual T Record
		{
			get
			{
				return _record;
			}

			set
			{
				if (_record != value)
				{
					FreeFields();

					_record = value;
				}
			}
		}

		#endregion

		#region Protected Methods

		#region Interface IHelper

		protected virtual void FreeFields()
		{
			Fields = null;

			NameField = null;
		}

		protected virtual IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>();
			}

			return Fields;
		}

		protected virtual IField GetField(string name)
		{
			IField result;

			if (string.IsNullOrWhiteSpace(name))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			result = GetFields().FirstOrDefault(f => f.Name == name);

		Cleanup:

			return result;
		}

		protected virtual IField GetField(long listNum)
		{
			IField result;

			if (listNum == 0)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			result = GetFields().FirstOrDefault(f => f.ListNum == listNum);

		Cleanup:

			return result;
		}

		protected virtual IField GetNameField()
		{
			if (NameField == null)
			{
				NameField = Globals.CreateInstance<IField>(x =>
				{
					x.Name = "Name";
					x.GetPrintedName = () => "Name";
					x.Validate = null;
					x.PrintDesc = null;
					x.List = null;
					x.Input = null;
					x.BuildValue = null;
					x.GetValue = () => Record.Name;
				});
			}

			return NameField;
		}

		protected virtual bool ValidateField(IField field, IValidateArgs args)
		{
			bool result;

			if (field == null || args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			if (field.Validate != null)
			{
				args.Clear();

				result = field.Validate(field, args);

				if (result == false)
				{
					args.ErrorField = field;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateFieldInterdependencies(IField field, IValidateArgs args)
		{
			bool result;

			if (field == null || args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			if (field.ValidateInterdependencies != null)
			{
				args.Clear();

				result = field.ValidateInterdependencies(field, args);

				if (result == false)
				{
					args.ErrorField = field;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual void PrintFieldDesc(IField field, bool editRec, bool editField, Enums.FieldDesc fieldDesc)
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

		protected virtual void PrintFieldDesc(IField field, IPrintDescArgs args)
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

		protected virtual void ListField(IField field, IListArgs args)
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

		protected virtual void InputField(IField field, IInputArgs args)
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

		#region Public Methods

		#region Interface IHelper

		public virtual string GetFieldName(long listNum)
		{
			var field = GetField(listNum);

			return field != null ? field.Name : null;
		}

		public virtual IList<string> GetFieldNames(Func<string, bool> matchFunc = null)
		{
			if (matchFunc == null)
			{
				matchFunc = fn => true;
			}

			return GetFields().Select(f => f.Name).Where(matchFunc).ToList();
		}

		public virtual string GetErrorFieldName(IValidateArgs args)
		{
			return args != null && args.ErrorField != null ? args.ErrorField.Name : null;
		}

		public virtual object GetErrorFieldValue(IValidateArgs args)
		{
			return args != null && args.ErrorField != null ? args.ErrorField.GetValue() : null;
		}

		public virtual bool ValidateRecord(IValidateArgs args)
		{
			bool result;

			if (args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			var fields = GetFields();

			foreach (var f in fields)
			{
				result = ValidateField(f, args);

				if (result == false)
				{
					break;
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool ValidateField(string fieldName, IValidateArgs args)
		{
			return ValidateField(GetField(fieldName), args);
		}

		public virtual bool ValidateRecordInterdependencies(IValidateArgs args)
		{
			bool result;

			if (args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			var fields = GetFields();

			foreach (var f in fields)
			{
				result = ValidateFieldInterdependencies(f, args);

				if (result == false)
				{
					break;
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool ValidateFieldInterdependencies(string fieldName, IValidateArgs args)
		{
			return ValidateFieldInterdependencies(GetField(fieldName), args);
		}

		public virtual void PrintFieldDesc(string fieldName, bool editRec, bool editField, Enums.FieldDesc fieldDesc)
		{
			PrintFieldDesc(GetField(fieldName), editRec, editField, fieldDesc);
		}

		public virtual void PrintFieldDesc(string fieldName, IPrintDescArgs args)
		{
			PrintFieldDesc(GetField(fieldName), args);
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

		public virtual void ListField(string fieldName, IListArgs args)
		{
			ListField(GetField(fieldName), args);
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

		public virtual void InputField(string fieldName, IInputArgs args)
		{
			InputField(GetField(fieldName), args);
		}

		#endregion

		#endregion
	}
}
