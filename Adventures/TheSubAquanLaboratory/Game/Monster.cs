
// Monster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(Eamon.Framework.IMonster))]
	public class Monster : Eamon.Game.Monster, Framework.IMonster
	{
		public virtual bool IsAndroid()
		{
			return (Uid > 1 && Uid < 7) || (Uid > 8 && Uid < 13) || (Uid > 13 && Uid < 16) || (Uid > 19 && Uid < 23);
		}

		public override string GetArmorDescString()
		{
			var armorDesc = "armor";

			if (IsInRoomLit())
			{
				if (Uid == 1)
				{
					armorDesc = "its plate-like hide";
				}
				else if (IsAndroid())
				{
					armorDesc = "its synthesized skin";
				}
				else if (Uid == 7 || Uid == 8)
				{
					armorDesc = "its rubbery grey skin";
				}
				else if (Uid == 13)
				{
					armorDesc = "its fish-skin armor";
				}
				else if (Uid == 19)
				{
					armorDesc = "his coarse fur";
				}
			}

			return armorDesc;
		}
	}
}
