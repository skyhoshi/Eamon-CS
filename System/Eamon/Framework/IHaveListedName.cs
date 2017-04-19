
// IHaveListedName.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IHaveListedName
	{
		string Name { get; set; }

		string[] Synonyms { get; set; }

		bool Seen { get; set; }

		Enums.ArticleType ArticleType { get; set; }

		string GetPluralName(IField field, StringBuilder buf);

		string GetPluralName01(StringBuilder buf);

		string GetDecoratedName(IField field, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName);

		IField GetNameField();
	}
}
