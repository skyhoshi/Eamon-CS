
// MonsterBattleState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IMonsterBattleState))]
	public class MonsterBattleState : EamonRT.Game.States.MonsterBattleState, IMonsterBattleState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			if (monster.Uid == 22)
			{
				if (Globals.Engine.CheckNBTLHostility(monster))
				{
					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterAttacksFoeState>(x =>
					{
						x.MemberNumber = MemberNumber;
					});
				}
				else
				{
					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterLoopIncrementState>();
				}

				Globals.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
