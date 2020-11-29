
// MonsterUsesNaturalWeaponsCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterUsesNaturalWeaponsCheckState : State, IMonsterUsesNaturalWeaponsCheckState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			if (LoopMonster.CombatCode == CombatCode.NaturalWeapons && LoopMonster.Weapon < 0)
			{
				LoopMonster.Weapon = 0;
			}

			if (LoopMonster.CheckNBTLHostility())
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
			Uid = 22;

			Name = "MonsterUsesNaturalWeaponsCheckState";
		}
	}
}
