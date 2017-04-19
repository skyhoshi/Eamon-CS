
// IEditable.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Args;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.DataEntry
{
	public interface IEditable
	{
		#region Methods

		void PrintFieldDesc(IField field, bool editRec, bool editField, Enums.FieldDesc fieldDesc);

		void PrintFieldDesc(IField field, IPrintDescArgs args);

		void ListRecord(bool fullDetail, bool showDesc, bool resolveEffects, bool lookupMsg, bool numberFields, bool excludeROFields);

		void ListRecord(IListArgs args);

		void ListField(IField field, IListArgs args);

		void ListErrorField(IValidateArgs args);

		void InputRecord(bool editRec, Enums.FieldDesc fieldDesc);

		void InputRecord(IInputArgs args);

		void InputField(IField field, IInputArgs args);

		#endregion
	}
}
