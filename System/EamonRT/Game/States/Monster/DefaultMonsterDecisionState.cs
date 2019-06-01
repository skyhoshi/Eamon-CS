
// DefaultMonsterDecisionState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
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

			if (monster.CheckNBTLHostility())
			{
				if (monster.CanMoveToRoom(true) && !monster.CheckCourage())
				{
					NextState = Globals.CreateInstance<IBeforeMonsterFleesRoomState>();

					goto Cleanup;
				}
				else if (monster.CombatCode != CombatCode.NeverFights)
				{
					NextState = Globals.CreateInstance<IMemberLoopInitializeState>();

					goto Cleanup;
				}
			}
			else if (monster.ShouldReadyWeapon())
			{
				NextState = Globals.CreateInstance<IArtifactLoopInitializeState>();

				goto Cleanup;
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
