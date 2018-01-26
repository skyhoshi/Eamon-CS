
// DefaultMonsterDecisionState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class DefaultMonsterDecisionState : State, IDefaultMonsterDecisionState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			if (monster.Location == Globals.GameState.Ro)
			{
				if (Globals.Engine.CheckNBTLHostility(monster))
				{
					if (monster.CanMoveToRoom(true) && !Globals.Engine.CheckCourage(monster))
					{
						NextState = Globals.CreateInstance<IMonsterFleesRoomState>();

						goto Cleanup;
					}
					else if (monster.CombatCode != Enums.CombatCode.NeverFights)
					{
						NextState = Globals.CreateInstance<IMonsterBattleState>();

						goto Cleanup;
					}
				}
				else
				{
					if ((monster.CombatCode == Enums.CombatCode.NaturalWeapons && monster.Weapon <= 0) || (monster.CombatCode == Enums.CombatCode.Weapons && monster.Weapon < 0))
					{
						NextState = Globals.CreateInstance<IMonsterReadiesWeaponState>(x =>
						{
							x.ArtifactList = Globals.Engine.GetReadyableWeaponList(monster);
						});

						goto Cleanup;
					}
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public DefaultMonsterDecisionState()
		{
			Name = "DefaultMonsterDecisionState";
		}
	}
}
