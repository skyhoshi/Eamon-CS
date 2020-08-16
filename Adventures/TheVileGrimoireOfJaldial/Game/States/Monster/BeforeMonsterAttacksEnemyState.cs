
// BeforeMonsterAttacksEnemyState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class BeforeMonsterAttacksEnemyState : EamonRT.Game.States.BeforeMonsterAttacksEnemyState, IBeforeMonsterAttacksEnemyState
	{
		public override void Execute()
		{
			base.Execute();

			// efreeti has special attack behavior when water weird is present

			if (Globals.NextState is IAttackCommand attackCommand && attackCommand.ActorMonster.Uid == 50 && attackCommand.AttackNumber == 1)
			{
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				attackCommand.ActorMonster.AttackCount = attackCommand.DobjMonster?.Uid == 38 ? 1 : waterWeirdMonster.IsInRoom(attackCommand.ActorRoom) ? 3 : -3;
			}
		}
	}
}
