
// BeforeMonsterFleesRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
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
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null && LoopMonster.Friendliness != Friendliness.Neutral);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			RedirectCommand = Globals.CreateInstance<IFleeCommand>();

			RedirectCommand.ActorMonster = LoopMonster;

			RedirectCommand.ActorRoom = LoopMonsterRoom;

			Globals.LoopGroupCount = LoopMonster.GroupCount;

			RedirectCommand.NextState = Globals.CreateInstance<IAfterMonsterFleesRoomState>();

			NextState = RedirectCommand;

			Globals.NextState = NextState;
		}

		public BeforeMonsterFleesRoomState()
		{
			Name = "BeforeMonsterFleesRoomState";
		}
	}
}
