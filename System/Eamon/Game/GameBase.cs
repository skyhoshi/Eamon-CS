
// GameBase.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	public abstract class GameBase : IGameBase
	{
		#region Protected Properties

		[ExcludeFromSerialization]
		protected virtual IList<IField> Fields { get; set; }

		[ExcludeFromSerialization]
		protected virtual IField NameField { get; set; }

		[ExcludeFromSerialization]
		protected virtual Action<bool> SetUidIfInvalid { get; set; }

		#endregion

		#region Public Properties

		#region Interface IGameBase

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		public virtual string Name { get; set; }

		public virtual string Desc { get; set; }

		public virtual string[] Synonyms { get; set; }

		public virtual bool Seen { get; set; }

		public virtual Enums.ArticleType ArticleType { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected abstract void Dispose(bool disposing);

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public void Dispose()      // virtual intentionally omitted
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Interface IGameBase

		public virtual void FreeFields()
		{
			Fields = null;

			NameField = null;
		}

		public virtual IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>();
			}

			return Fields;
		}

		public virtual IField GetField(string name)
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

		public virtual IField GetField(long listNum)
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

		public virtual void SetParentReferences()
		{
			// do nothing
		}

		public virtual string GetPluralName(IField field, StringBuilder buf)
		{
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetPluralName01(StringBuilder buf)
		{
			return GetPluralName(GetField("Name"), buf);
		}

		public virtual string GetDecoratedName(IField field, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.None, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), ArticleType, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.The, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf.Clear();

		Cleanup:

			return rc;
		}

		public virtual IField GetNameField()
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
					x.GetValue = () => Name;
				});
			}

			return NameField;
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

		public virtual bool ValidateField(IField field, IValidateArgs args)
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

		public virtual bool ValidateFieldInterdependencies(IField field, IValidateArgs args)
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

		#region Class GameBase

		public GameBase()
		{
			IsUidRecycled = true;

			Name = "";

			Desc = "";
		}

		#endregion

		#endregion
	}
}
