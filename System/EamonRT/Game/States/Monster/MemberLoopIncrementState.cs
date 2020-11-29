
// MemberLoopIncrementState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MemberLoopIncrementState : State, IMemberLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual long MaxMemberAttackCount { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			MaxMemberAttackCount = LoopMonster.GetMaxMemberAttackCount();

			Debug.Assert(Globals.LoopMemberNumber >= 0 && Globals.LoopMemberNumber <= LoopMonster.GroupCount && Globals.LoopMemberNumber <= MaxMemberAttackCount);

			Globals.LoopMemberNumber++;

			if (Globals.LoopMemberNumber > LoopMonster.GroupCount || Globals.LoopMemberNumber > MaxMemberAttackCount)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAttackLoopInitializeState>();
			}

			Globals.NextState = NextState;
		}

		public MemberLoopIncrementState()
		{
			Uid = 16;

			Name = "MemberLoopIncrementState";
		}
	}
}
