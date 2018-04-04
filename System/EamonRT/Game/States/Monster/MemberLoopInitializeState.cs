
// MemberLoopInitializeState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MemberLoopInitializeState : State, IMemberLoopInitializeState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

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
