
// Engine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : Eamon.Game.Engine, IEngine
	{
		public Engine()
		{
			MissDescs[new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 1)] = "Parried";

			MissDescs[new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 1)] = "Parried";

			MissDescs[new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 1)] = "Parried";
		}
	}
}
