
// BeforeMonsterAttacksEnemyState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
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
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			if (monster.Weapon < 0)
			{
				Globals.LoopAttackNumber = Math.Abs(monster.AttackCount);

				Globals.LoopMemberNumber = monster.GroupCount;

				goto Cleanup;
			}

			var monsterList = Globals.Engine.GetHostileMonsterList(monster);

			Debug.Assert(monsterList != null);

			if (monsterList.Count < 1)
			{
				Globals.LoopAttackNumber = Math.Abs(monster.AttackCount);

				Globals.LoopMemberNumber = monster.GroupCount;

				goto Cleanup;
			}

			if (monster.AttackCount > 1 && Globals.LoopLastDfMonster != null && !monsterList.Contains(Globals.LoopLastDfMonster))
			{
				Globals.LoopAttackNumber = Math.Abs(monster.AttackCount);

				goto Cleanup;
			}

			var rl = Globals.Engine.RollDice(1, monsterList.Count, 0);

			var command = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.MemberNumber = Globals.LoopMemberNumber;

				x.AttackNumber = Globals.LoopAttackNumber;
			});

			command.ActorMonster = monster;

			command.ActorRoom = room;

			command.Dobj = monster.AttackCount > 1 && Globals.LoopLastDfMonster != null ? Globals.LoopLastDfMonster : monsterList[(int)rl - 1];

			Globals.LoopLastDfMonster = command.Dobj as IMonster;

			command.NextState = Globals.CreateInstance<IAttackLoopIncrementState>();

			NextState = command;

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public BeforeMonsterAttacksEnemyState()
		{
			Name = "BeforeMonsterAttacksEnemyState";
		}
	}
}
