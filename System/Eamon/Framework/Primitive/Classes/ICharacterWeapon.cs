
// ICharacterWeapon.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface ICharacterWeapon
	{
		ICharacter Parent { get; set; }

		string Name { get; set; }

		bool IsPlural { get; set; }

		Enums.PluralType PluralType { get; set; }

		Enums.ArticleType ArticleType { get; set; }

		long Complexity { get; set; }

		Enums.Weapon Type { get; set; }

		long Dice { get; set; }

		long Sides { get; set; }

		bool IsActive();
	}
}
