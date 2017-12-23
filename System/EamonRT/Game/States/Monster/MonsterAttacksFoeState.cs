
// MonsterAttacksFoeState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterAttacksFoeState : State, IMonsterAttacksFoeState
	{
		public virtual long MemberNumber { get; set; }

		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null && monster.CombatCode != Enums.CombatCode.NeverFights && monster.Friendliness != Enums.Friendliness.Neutral);

			Debug.Assert(MemberNumber >= 1 && MemberNumber <= monster.GroupCount && MemberNumber <= 5);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var monsterList = Globals.Engine.GetHostileMonsterList(monster);

			Debug.Assert(monsterList != null);

			if (monsterList.Count < 1)
			{
				goto Cleanup;
			}

			var rl = Globals.Engine.RollDice01(1, monsterList.Count, 0);

			var command = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.MemberNumber = MemberNumber;
			});

			command.ActorMonster = monster;

			command.ActorRoom = room;

			command.DobjMonster = monsterList[(int)rl - 1];

			command.NextState = Globals.CreateInstance<IMonsterBattleState>(x =>
			{
				x.MemberNumber = MemberNumber + 1;
			});

			NextState = command;

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterBattleState>(x =>
				{
					x.MemberNumber = MemberNumber + 1;
				});
			}

			Globals.NextState = NextState;
		}

		public MonsterAttacksFoeState()
		{
			Name = "MonsterAttacksFoeState";
		}
	}
}
