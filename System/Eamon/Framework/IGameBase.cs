
// IGameBase.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Text;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IGameBase : IDisposable
	{
		#region Properties

		/// <summary></summary>
		long Uid { get; set; }

		/// <summary></summary>
		bool IsUidRecycled { get; set; }

		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string Desc { get; set; }

		/// <summary></summary>
		string[] Synonyms { get; set; }

		/// <summary></summary>
		bool Seen { get; set; }

		/// <summary></summary>
		ArticleType ArticleType { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		void SetParentReferences();

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetPluralName(string fieldName, StringBuilder buf);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetPluralName01(StringBuilder buf);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <param name="articleType"></param>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		/// <summary></summary>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		/// <summary></summary>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		/// <summary></summary>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="showName"></param>
		/// <returns></returns>
		RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName);

		#endregion
	}
}
