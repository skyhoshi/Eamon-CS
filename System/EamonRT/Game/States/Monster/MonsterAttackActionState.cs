
// MonsterAttackActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterAttackActionState : State, IMonsterAttackActionState
	{
		/// <summary></summary>
		public virtual IList<IMonster> HostileMonsterList { get; set; }

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		/// <summary></summary>
		public virtual long HostileMonsterListIndex { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			HostileMonsterList = gEngine.GetHostileMonsterList(LoopMonster);

			Debug.Assert(HostileMonsterList != null);

			if (HostileMonsterList.Count < 1)
			{
				NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();

				goto Cleanup;
			}

			if (LoopMonster.AttackCount > 1 && Globals.LoopLastDfMonster != null && !HostileMonsterList.Contains(Globals.LoopLastDfMonster))
			{
				NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();

				goto Cleanup;
			}

			HostileMonsterListIndex = gEngine.RollDice(1, HostileMonsterList.Count, -1);

			ActionCommand = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.NextState = Globals.CreateInstance<IMonsterAttackLoopIncrementState>();

				x.ActorMonster = LoopMonster;

				x.ActorRoom = LoopMonsterRoom;

				x.MemberNumber = Globals.LoopMemberNumber;

				x.AttackNumber = Globals.LoopAttackNumber;

				x.Dobj = LoopMonster.AttackCount > 1 && Globals.LoopLastDfMonster != null ? Globals.LoopLastDfMonster : HostileMonsterList[(int)HostileMonsterListIndex];
			});

			Globals.LoopLastDfMonster = ActionCommand.Dobj as IMonster;

			ActionCommand.MonsterExecute();

			NextState = ActionCommand.NextState;

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterAttackActionState()
		{
			Uid = 8;

			Name = "MonsterAttackActionState";
		}
	}
}
