
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		protected override void PrintHealthStatus()
		{
			var monsterDies = DfMonster.IsDead();

			var deadBodyArtifact = DfMonster.DeadBody > 0 ? Globals.ADB[DfMonster.DeadBody] : null;

			// Desc of dead body; set flag as seen

			if (DfMonster.GroupCount == 1 && deadBodyArtifact != null && monsterDies)
			{
				if (!BlastSpell)
				{
					Globals.Out.WriteLine();
				}

				Globals.Buf.Clear();

				deadBodyArtifact.BuildPrintedFullDesc(Globals.Buf, false);

				Globals.Out.Write("{0}", Globals.Buf);

				deadBodyArtifact.Seen = true;
			}

			base.PrintHealthStatus();
		}

		public override void ExecuteAttack()
		{
			Debug.Assert(DfMonster != null);

			// Allow cursing if defender is enemy

			if (DfMonster.Friendliness == Friendliness.Enemy)
			{
				Globals.MonsterCurses = true;
			}

			base.ExecuteAttack();

			Globals.MonsterCurses = false;
		}
	}
}
