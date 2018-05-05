
// BeforeMonsterAttacksEnemyState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class BeforeMonsterAttacksEnemyState : EamonRT.Game.States.BeforeMonsterAttacksEnemyState, EamonRT.Framework.States.IBeforeMonsterAttacksEnemyState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			// Necromancer has special attack routine. Casts spells, never misses, always attacks player.

			if (monster.Uid == 22)
			{
				NextState = Globals.CreateInstance<IBeforeNecromancerAttacksEnemyState>();

				Globals.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
