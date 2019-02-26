
// BeforeMonsterFleesRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforeMonsterFleesRoomState : State, IBeforeMonsterFleesRoomState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.Friendliness != Friendliness.Neutral);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var command = Globals.CreateInstance<IFleeCommand>();

			command.ActorMonster = monster;

			command.ActorRoom = room;

			Globals.LoopGroupCount = monster.GroupCount;

			command.NextState = Globals.CreateInstance<IAfterMonsterFleesRoomState>();

			NextState = command;

			Globals.NextState = NextState;
		}

		public BeforeMonsterFleesRoomState()
		{
			Name = "BeforeMonsterFleesRoomState";
		}
	}
}
