
// Monster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IMonster))]
	public class Monster : Eamon.Game.Monster, Framework.IMonster
	{
		public override string[] GetNaturalAttackDescs()      // Note: much more to implement here
		{
			var attackDescs = base.GetNaturalAttackDescs();

			if (Uid == 50)
			{
				attackDescs = new string[] { "touches" };
			}

			return attackDescs;
		}

		public override string GetArmorDescString()		// Note: much more to implement here
		{
			var armorDesc = base.GetArmorDescString();

			if (IsInRoomLit())
			{
				if (Uid == 50)
				{
					armorDesc = "its fiery skin";
				}
			}

			return armorDesc;
		}
	}
}
