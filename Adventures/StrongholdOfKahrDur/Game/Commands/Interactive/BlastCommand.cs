﻿
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void PlayerProcessEvents(EventType eventType)
		{
			var helmArtifact = gADB[25];

			Debug.Assert(helmArtifact != null);

			// Necromancer cannot be blasted unless wearing Wizard's Helm

			if (eventType == EventType.AfterMonsterGetsAggravated && DobjMonster != null && DobjMonster.Uid == 22 && !helmArtifact.IsWornByCharacter())
			{
				var rl = gEngine.RollDice(1, 4, 56);

				gEngine.PrintEffectDesc(rl);

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
				var helmArtifact = gADB[25];

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
