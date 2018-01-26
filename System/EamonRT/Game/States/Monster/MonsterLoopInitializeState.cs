
// MonsterLoopInitializeState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterLoopInitializeState : State, IMonsterLoopInitializeState
	{
		public override void Execute()
		{
			Globals.LoopMonsterUid = 0;

			var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

			foreach (var fv in friendlinessValues)
			{
				Debug.Assert(Globals.GameState.GetNBTL(fv) >= 0);

				if (Globals.IsRulesetVersion(5))
				{
					Debug.Assert(Globals.GameState.GetDTTL(fv) >= 0);
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterLoopInitializeState()
		{
			Name = "MonsterLoopInitializeState";
		}
	}
}
