
// DefaultMonsterDecisionState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
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
			RetCode rc;

			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			if (monster.Location == Globals.GameState.Ro)
			{
				if (Globals.RtEngine.CheckNBTLHostility(monster))
				{
					var s = 0L;

					if (monster.DmgTaken > 0 || monster.OrigGroupCount > monster.GroupCount)
					{
						s++;
					}

					if (monster.DmgTaken + 4 >= monster.Hardiness)
					{
						s++;
					}

					var rl = 0L;

					rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rl += (s * 5);

					if (monster.CanMoveToRoom(true) && rl > monster.Courage)        // monster.Courage < 100 &&
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
							x.ArtifactList = Globals.RtEngine.GetReadyableWeaponList(monster);
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
