
// IGameBase.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Text;
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

		void SetParentReferences();

		string GetPluralName(string fieldName, StringBuilder buf);

		string GetPluralName01(StringBuilder buf);

		string GetDecoratedName(string fieldName, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName);

		#endregion
	}
}
