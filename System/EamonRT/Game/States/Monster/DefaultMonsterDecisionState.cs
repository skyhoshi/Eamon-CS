
// DefaultMonsterDecisionState.cs

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
	public class DefaultMonsterDecisionState : State, IDefaultMonsterDecisionState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			if (LoopMonster.CheckNBTLHostility())
			{
				if (LoopMonster.CanMoveToRoom(true) && !LoopMonster.CheckCourage())
				{
					NextState = Globals.CreateInstance<IBeforeMonsterFleesRoomState>();

					goto Cleanup;
				}
				else if (LoopMonster.CombatCode != CombatCode.NeverFights)
				{
					NextState = Globals.CreateInstance<IMemberLoopInitializeState>();

					goto Cleanup;
				}
			}
			else if (LoopMonster.ShouldReadyWeapon())
			{
				NextState = Globals.CreateInstance<IWeaponArtifactLoopInitializeState>();

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
			Uid = 15;

			Name = "DefaultMonsterDecisionState";
		}
	}
}
