
// MonsterUsesNaturalWeaponsCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterUsesNaturalWeaponsCheckState : State, IMonsterUsesNaturalWeaponsCheckState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			if (monster.CombatCode == Enums.CombatCode.NaturalWeapons && monster.Weapon < 0)
			{
				monster.Weapon = 0;
			}

			if (Globals.Engine.CheckNBTLHostility(monster))
			{
				NextState = Globals.CreateInstance<IMonsterReadiedWeaponCheckState>();
			
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterUsesNaturalWeaponsCheckState()
		{
			Name = "MonsterUsesNaturalWeaponsCheckState";
		}
	}
}
