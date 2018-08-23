
// DefaultMonsterDecisionState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class DefaultMonsterDecisionState : State, IDefaultMonsterDecisionState
	{
		public virtual bool ShouldMonsterRearm(IMonster monster)
		{
			return true;
		}

		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			if (Globals.Engine.CheckNBTLHostility(monster))
			{
				if (monster.CanMoveToRoom(true) && !Globals.Engine.CheckCourage(monster))
				{
					NextState = Globals.CreateInstance<IBeforeMonsterFleesRoomState>();

					goto Cleanup;
				}
				else if (monster.CombatCode != Enums.CombatCode.NeverFights)
				{
					NextState = Globals.CreateInstance<IMemberLoopInitializeState>();

					goto Cleanup;
				}
			}
			else if (ShouldMonsterRearm(monster))
			{
				NextState = Globals.CreateInstance<IArtifactLoopInitializeState>();

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
			Name = "DefaultMonsterDecisionState";
		}
	}
}

/* EamonCsCodeTemplate

// DefaultMonsterDecisionState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class DefaultMonsterDecisionState : EamonRT.Game.States.DefaultMonsterDecisionState, IDefaultMonsterDecisionState
	{

	}
}
EamonCsCodeTemplate */
