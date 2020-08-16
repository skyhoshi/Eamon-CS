
// AfterMonsterFleesRoomState.cs

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
	public class AfterMonsterFleesRoomState : State, IAfterMonsterFleesRoomState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null && LoopMonster.Friendliness != Friendliness.Neutral);

			if (LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.GroupCount < Globals.LoopGroupCount)
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
