
// IHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.Args;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Helpers.Generic
{
	public interface IHelper<T> where T : class, IGameBase
	{
		#region Properties

		T Record { get; set; }

		#endregion

		#region Methods

		string GetFieldName(long listNum);

		IList<string> GetFieldNames(Func<string, bool> matchFunc = null);

		string GetErrorFieldName(IValidateArgs args);

		object GetErrorFieldValue(IValidateArgs args);

		bool ValidateRecord(IValidateArgs args);

		bool ValidateField(string fieldName, IValidateArgs args);

		bool ValidateRecordInterdependencies(IValidateArgs args);

		bool ValidateFieldInterdependencies(string fieldName, IValidateArgs args);

		void PrintFieldDesc(string fieldName, bool editRec, bool editField, Enums.FieldDesc fieldDesc);

		void PrintFieldDesc(string fieldName, IPrintDescArgs args);

		void ListRecord(bool fullDetail, bool showDesc, bool resolveEffects, bool lookupMsg, bool numberFields, bool excludeROFields);

		void ListRecord(IListArgs args);

		void ListField(string fieldName, IListArgs args);

		void ListErrorField(IValidateArgs args);

		void InputRecord(bool editRec, Enums.FieldDesc fieldDesc);

		void InputRecord(IInputArgs args);

		void InputField(string fieldName, IInputArgs args);

		#endregion
	}
}
