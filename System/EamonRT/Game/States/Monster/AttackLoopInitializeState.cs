
// AttackLoopInitializeState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AttackLoopInitializeState : State, IAttackLoopInitializeState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.CombatCode != Enums.CombatCode.NeverFights && monster.Friendliness != Enums.Friendliness.Neutral);

			Globals.LoopAttackNumber = 0;

			Globals.LoopGroupCount = monster.GroupCount;

			Globals.LoopLastDfMonster = null;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public AttackLoopInitializeState()
		{
			Name = "AttackLoopInitializeState";
		}
	}
}

/* EamonCsCodeTemplate

// AttackLoopInitializeState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class AttackLoopInitializeState : EamonRT.Game.States.AttackLoopInitializeState, IAttackLoopInitializeState
	{

	}
}
EamonCsCodeTemplate */
