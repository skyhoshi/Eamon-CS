
// AfterMonsterFleesRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AfterMonsterFleesRoomState : State, IAfterMonsterFleesRoomState
	{
		public override void Execute()
		{
			var monster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.Friendliness != Friendliness.Neutral);

			if (monster.CombatCode != CombatCode.NeverFights && monster.GroupCount < Globals.LoopGroupCount)
			{
				NextState = Globals.CreateInstance<IMemberLoopInitializeState>();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public AfterMonsterFleesRoomState()
		{
			Name = "AfterMonsterFleesRoomState";
		}
	}
}
