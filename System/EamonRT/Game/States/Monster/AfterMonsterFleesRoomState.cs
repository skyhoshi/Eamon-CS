
// AfterMonsterFleesRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AfterMonsterFleesRoomState : State, IAfterMonsterFleesRoomState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.Friendliness != Enums.Friendliness.Neutral);

			if (monster.CombatCode != Enums.CombatCode.NeverFights && monster.GroupCount < Globals.LoopGroupCount)
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

/* EamonCsCodeTemplate

// AfterMonsterFleesRoomState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class AfterMonsterFleesRoomState : EamonRT.Game.States.AfterMonsterFleesRoomState, IAfterMonsterFleesRoomState
	{

	}
}
EamonCsCodeTemplate */
