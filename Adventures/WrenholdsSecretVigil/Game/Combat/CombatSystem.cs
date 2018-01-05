
// CombatSystem.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Combat;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Combat
{
	[ClassMappings(typeof(EamonRT.Framework.Combat.ICombatSystem))]
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

				Globals.Out.Write(Globals.Buf);

				deadBodyArtifact.Seen = true;
			}

			base.PrintHealthStatus();
		}

		public override void ExecuteAttack()
		{
			Globals.MonsterCurses = true;

			base.ExecuteAttack();

			Globals.MonsterCurses = false;
		}
	}
}
