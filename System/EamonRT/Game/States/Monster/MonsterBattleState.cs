
// MonsterBattleState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterBattleState : State, IMonsterBattleState
	{
		public virtual bool ReadyCommandCalled { get; set; }

		public virtual long MemberNumber { get; set; }

		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.CombatCode != Enums.CombatCode.NeverFights && monster.Friendliness != Enums.Friendliness.Neutral);

			if (MemberNumber < 1 || MemberNumber > monster.GroupCount || MemberNumber > 5)
			{
				goto Cleanup;
			}

			if (monster.Weapon < 0 || (monster.CombatCode == Enums.CombatCode.NaturalWeapons && monster.Weapon == 0 && !ReadyCommandCalled))
			{
				NextState = Globals.CreateInstance<IMonsterReadiesWeaponState>(x =>
				{
					x.ArtifactList = Globals.RtEngine.GetReadyableWeaponList(monster);

					x.MemberNumber = MemberNumber;
				});

				goto Cleanup;
			}

			if (monster.Weapon >= 0 && Globals.RtEngine.CheckNBTLHostility(monster))
			{
				NextState = Globals.CreateInstance<IMonsterAttacksFoeState>(x =>
				{
					x.MemberNumber = MemberNumber;
				});

				goto Cleanup;
			}

			NextState = Globals.CreateInstance<IMonsterBattleState>(x =>
			{
				x.ReadyCommandCalled = ReadyCommandCalled;

				x.MemberNumber = MemberNumber + 1;
			});

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterBattleState()
		{
			Name = "MonsterBattleState";

			MemberNumber = 1;
		}
	}
}
