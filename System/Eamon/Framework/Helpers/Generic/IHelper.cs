
// IHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Helpers.Generic
{
	public interface IHelper<T> where T : class, IGameBase
	{
		#region Properties

		T Record { get; set; }

		long Index { get; set; }

		StringBuilder Buf { get; set; }

		StringBuilder Buf01 { get; set; }

		bool EditRec { get; set; }

		bool EditField { get; set; }

		bool ShowDesc { get; set; }

		Enums.FieldDesc FieldDesc { get; set; }

		long BufSize { get; set; }

		char FillChar { get; set; }

		long Offset { get; set; }

		string ErrorFieldName { get; set; }

		string ErrorMessage { get; set; }

		Type RecordType { get; set; }

		IGameBase EditRecord { get; set; }

		long NewRecordUid { get; set; }

		bool FullDetail { get; set; }

		bool ResolveEffects { get; set; }

		bool LookupMsg { get; set; }

		bool NumberFields { get; set; }

		bool ExcludeROFields { get; set; }

		bool AddToListedNames { get; set; }

		long ListNum { get; set; }

		#endregion

		#region Methods

		string GetFieldName(string name);

		string GetFieldName(long listNum);

		IList<string> GetNames(Func<string, bool> matchFunc = null);

		string GetPrintedName(string fieldName);

		string GetName(string fieldName, bool addToNamesList = false);

		object GetValue(string fieldName);

		bool ValidateRecord();

		bool ValidateField(string fieldName);

		bool ValidateRecordInterdependencies();

		bool ValidateFieldInterdependencies(string fieldName);

		void PrintFieldDesc(string fieldName, bool editRec, bool editField, Enums.FieldDesc fieldDesc);

		void PrintFieldDesc(string fieldName);

		void ListRecord(bool fullDetail, bool showDesc, bool resolveEffects, bool lookupMsg, bool numberFields, bool excludeROFields);

		void ListRecord();

		void ListField(string fieldName);

		void ListErrorField();

		void InputRecord(bool editRec, Enums.FieldDesc fieldDesc);

		void InputRecord();

		void InputField(string fieldName);

		void Clear();

		#endregion
	}
}
