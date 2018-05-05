
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, EamonRT.Framework.Combat.ICombatSystem
	{
		protected override void PrintHealthStatus()
		{
			// Repetitive Spooks' repetitive death description

			var monsterDies = DfMonster.IsDead();

			if (DfMonster.Uid == 9 && monsterDies)
			{
				if (!BlastSpell)
				{
					Globals.Out.WriteLine();
				}

				Globals.Engine.PrintEffectDesc(8, false);
			}
			else
			{
				base.PrintHealthStatus();
			}
		}
	}
}
