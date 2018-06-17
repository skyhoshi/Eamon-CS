
// BlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var helmArtifact = Globals.ADB[25];

			Debug.Assert(helmArtifact != null);

			// Necromancer cannot be blasted unless wearing Wizard's Helm

			if (eventType == PpeAfterMonsterGetsAggravated && DobjMonster != null && DobjMonster.Uid == 22 && !helmArtifact.IsWornByCharacter())
			{
				var rl = Globals.Engine.RollDice01(1, 4, 56);

				Globals.Engine.PrintEffectDesc(rl);

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			// When Necromancer is blasted only allow skill increases if wearing Wizard's Helm

			if (DobjMonster != null && DobjMonster.Uid == 22)
			{
				var helmArtifact = Globals.ADB[25];

				Debug.Assert(helmArtifact != null);

				return helmArtifact.IsWornByCharacter();
			}
			else
			{
				return base.ShouldAllowSkillGains();
			}
		}
	}
}
