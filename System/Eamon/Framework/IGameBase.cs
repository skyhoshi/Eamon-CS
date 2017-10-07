
// IGameBase.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Args;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IGameBase : IDisposable
	{
		#region Properties

		long Uid { get; set; }

		bool IsUidRecycled { get; set; }

		string Name { get; set; }

		string Desc { get; set; }

		string[] Synonyms { get; set; }

		bool Seen { get; set; }

		Enums.ArticleType ArticleType { get; set; }

		#endregion

		#region Methods

		void FreeFields();

		IList<IField> GetFields();

		IField GetField(string name);

		IField GetField(long listNum);

		void SetParentReferences();

		string GetPluralName(IField field, StringBuilder buf);

		string GetPluralName01(StringBuilder buf);

		string GetDecoratedName(IField field, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName);

		IField GetNameField();

		bool ValidateRecord(IValidateArgs args);

		bool ValidateField(IField field, IValidateArgs args);

		bool ValidateRecordInterdependencies(IValidateArgs args);

		bool ValidateFieldInterdependencies(IField field, IValidateArgs args);

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
