
// CharacterWeapon.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class CharacterWeapon : ICharacterWeapon
	{
		[ExcludeFromSerialization]
		public virtual ICharacter Parent { get; set; }

		public virtual string Name { get; set; }

		public virtual bool IsPlural { get; set; }

		public virtual Enums.PluralType PluralType { get; set; }

		public virtual Enums.ArticleType ArticleType { get; set; }

		public virtual long Complexity { get; set; }

		public virtual Enums.Weapon Type { get; set; }

		public virtual long Dice { get; set; }

		public virtual long Sides { get; set; }

		public virtual bool IsActive()
		{
			return !string.IsNullOrWhiteSpace(Name) && !string.Equals(Name, "NONE", StringComparison.OrdinalIgnoreCase);
		}

		public CharacterWeapon()
		{
			Name = "NONE";
		}
	}
}
