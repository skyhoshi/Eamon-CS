
// MemberLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MemberLoopInitializeState : State, IMemberLoopInitializeState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Globals.LoopMemberNumber = 0;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMemberLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MemberLoopInitializeState()
		{
			Name = "MemberLoopInitializeState";
		}
	}
}
