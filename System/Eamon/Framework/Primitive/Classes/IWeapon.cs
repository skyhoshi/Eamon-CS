
// IWeapon.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IWeapon
	{
		string Name { get; set; }

		string EmptyVal { get; set; }

		string MarcosName { get; set; }

		bool MarcosIsPlural { get; set; }

		Enums.PluralType MarcosPluralType { get; set; }

		Enums.ArticleType MarcosArticleType { get; set; }

		long MarcosPrice { get; set; }

		long MarcosDice { get; set; }

		long MarcosSides { get; set; }

		long MinValue { get; set; }

		long MaxValue { get; set; }
	}
}
