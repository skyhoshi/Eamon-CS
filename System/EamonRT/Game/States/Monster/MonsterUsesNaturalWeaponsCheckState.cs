
// MonsterUsesNaturalWeaponsCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
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

			if (monster.CombatCode == CombatCode.NaturalWeapons && monster.Weapon < 0)
			{
				monster.Weapon = 0;
			}

			if (monster.CheckNBTLHostility())
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
