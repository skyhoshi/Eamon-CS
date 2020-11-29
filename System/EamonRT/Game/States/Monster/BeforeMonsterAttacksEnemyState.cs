
// BeforeMonsterAttacksEnemyState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
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
	public class BeforeMonsterAttacksEnemyState : State, IBeforeMonsterAttacksEnemyState
	{
		/// <summary></summary>
		public virtual IList<IMonster> HostileMonsterList { get; set; }

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual long HostileMonsterListIndex { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			if (LoopMonster.Weapon < 0)
			{
				Globals.LoopAttackNumber = Math.Abs(LoopMonster.AttackCount);

				Globals.LoopMemberNumber = LoopMonster.GroupCount;

				goto Cleanup;
			}

			HostileMonsterList = gEngine.GetHostileMonsterList(LoopMonster);

			Debug.Assert(HostileMonsterList != null);

			if (HostileMonsterList.Count < 1)
			{
				Globals.LoopAttackNumber = Math.Abs(LoopMonster.AttackCount);

				Globals.LoopMemberNumber = LoopMonster.GroupCount;

				goto Cleanup;
			}

			if (LoopMonster.AttackCount > 1 && Globals.LoopLastDfMonster != null && !HostileMonsterList.Contains(Globals.LoopLastDfMonster))
			{
				Globals.LoopAttackNumber = Math.Abs(LoopMonster.AttackCount);

				goto Cleanup;
			}

			HostileMonsterListIndex = gEngine.RollDice(1, HostileMonsterList.Count, -1);

			RedirectCommand = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.MemberNumber = Globals.LoopMemberNumber;

				x.AttackNumber = Globals.LoopAttackNumber;
			});

			RedirectCommand.ActorMonster = LoopMonster;

			RedirectCommand.ActorRoom = LoopMonsterRoom;

			RedirectCommand.Dobj = LoopMonster.AttackCount > 1 && Globals.LoopLastDfMonster != null ? Globals.LoopLastDfMonster : HostileMonsterList[(int)HostileMonsterListIndex];

			Globals.LoopLastDfMonster = RedirectCommand.Dobj as IMonster;

			RedirectCommand.NextState = Globals.CreateInstance<IAttackLoopIncrementState>();

			NextState = RedirectCommand;

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public BeforeMonsterAttacksEnemyState()
		{
			Uid = 11;

			Name = "BeforeMonsterAttacksEnemyState";
		}
	}
}
