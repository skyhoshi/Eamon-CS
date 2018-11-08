
// ICharacterArtifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface ICharacterArtifact
	{
		ICharacter Parent { get; set; }

		string Name { get; set; }

		string Desc { get; set; }

		bool IsPlural { get; set; }

		Enums.PluralType PluralType { get; set; }

		Enums.ArticleType ArticleType { get; set; }

		long Value { get; set; }

		long Weight { get; set; }

		Enums.ArtifactType Type { get; set; }

		long Field1 { get; set; }

		long Field2 { get; set; }

		long Field3 { get; set; }

		long Field4 { get; set; }

		long Field5 { get; set; }

		bool IsActive();

		void ClearExtraFields();
	}
}
