
// MonsterFleesRoomState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterFleesRoomState : State, IMonsterFleesRoomState
	{
		public virtual bool FleeCommandCalled { get; set; }

		public virtual long GroupCount { get; set; }

		public override void Execute()
		{
			ICommand command = null;

			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.Friendliness != Enums.Friendliness.Neutral);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			if (!FleeCommandCalled)
			{
				command = Globals.CreateInstance<IFleeCommand>();

				command.ActorMonster = monster;

				command.ActorRoom = room;

				command.NextState = Globals.CreateInstance<IMonsterFleesRoomState>(x =>
				{
					x.FleeCommandCalled = true;

					x.GroupCount = monster.GroupCount;
				});

				NextState = command;
			}
			else
			{
				if (monster.GroupCount < GroupCount)
				{
					NextState = Globals.CreateInstance<IMonsterBattleState>();
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterFleesRoomState()
		{
			Name = "MonsterFleesRoomState";
		}
	}
}
