
// MonsterLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterLoopIncrementState : State, IMonsterLoopIncrementState
	{
		public override void Execute()
		{
			if (Globals.LoopMonsterUid > 0)
			{
				ProcessRevealContentArtifacts();
			}

			while (true)
			{
				Globals.LoopMonsterUid++;

				var monster = Globals.MDB[Globals.LoopMonsterUid];

				if (monster != null)
				{
					if (monster.ShouldProcessInGameLoop())
					{
						NextState = Globals.CreateInstance<IDefaultMonsterDecisionState>();

						goto Cleanup;
					}
				}
				else
				{
					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IEndOfRoundState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterLoopIncrementState()
		{
			Name = "MonsterLoopIncrementState";
		}
	}
}
