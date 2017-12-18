
// BlastCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IBlastCommand))]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		protected override void PlayerProcessEvents01()
		{
			var artifact = Globals.ADB[25];

			Debug.Assert(artifact != null);

			// Necromancer cannot be blasted unless wearing Wizard's Helm

			if (DobjMonster != null && DobjMonster.Uid == 22 && !artifact.IsWornByCharacter())
			{
				var rl = Globals.Engine.RollDice01(1, 4, 56);

				Globals.Engine.PrintEffectDesc(rl);

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents01();
			}
		}

		protected override bool IsSkillIncreaseAllowed()
		{
			// When Necromancer is blasted only allow skill increases if wearing Wizard's Helm

			if (DobjMonster != null && DobjMonster.Uid == 22)
			{
				var artifact = Globals.ADB[25];

				Debug.Assert(artifact != null);

				return artifact.IsWornByCharacter();
			}
			else
			{
				return base.IsSkillIncreaseAllowed();
			}
		}
	}
}
