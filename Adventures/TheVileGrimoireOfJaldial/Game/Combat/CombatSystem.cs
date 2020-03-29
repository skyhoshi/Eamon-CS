
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		protected virtual bool ScoredCriticalHit { get; set; }

		protected override void RollToHitOrMiss()
		{
			ScoredCriticalHit = false;

			base.RollToHitOrMiss();

			// Bloodnettle always hits when draining blood

			if (OfMonster.Uid == 20 && DfMonster.Uid == gGameState.BloodnettleVictimUid && _rl > _odds)
			{
				_rl = _odds;
			}
		}

		protected override void CheckArmor()
		{
			// Bloodnettle always injures when draining blood

			if (OfMonster.Uid == 20 && DfMonster.Uid == gGameState.BloodnettleVictimUid && _d2 < 1)
			{
				_d2 = 1;
			}

			base.CheckArmor();
		}

		protected override void CheckMonsterStatus()
		{
			base.CheckMonsterStatus();

			// Bloodnettle selects its next victim

			if (OfMonster.Uid == 20 && !DfMonster.IsInLimbo() && gGameState.BloodnettleVictimUid == 0)
			{
				gGameState.BloodnettleVictimUid = DfMonster.Uid;
			}
		}

		public override void ExecuteAttack()
		{
			var griffinMonster = Globals.MDB[40];

			Debug.Assert(griffinMonster != null);

			// Attacking baby griffins makes the parent angry

			if (DfMonster.Uid == 41 && !griffinMonster.IsInLimbo() && !gGameState.GriffinAngered)
			{
				if (griffinMonster.IsInRoomUid(gGameState.Ro) && griffinMonster.GetInRoom().IsLit())
				{
					gOut.Print("The parent griffin is enraged by your attacks on the griffin cubs!");
				}

				gGameState.GriffinAngered = true;
			}

			base.ExecuteAttack();
		}
	}
}
