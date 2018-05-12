
// BlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		protected override void PlayerProcessEvents()
		{
			// BLAST Bozworth

			if (DobjMonster != null && DobjMonster.Uid == 20)
			{
				Globals.Engine.PrintEffectDesc(21);

				DobjMonster.SetInLimbo();

				Globals.Engine.CheckEnemies();

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected override bool ShouldAllowSkillGains()
		{
			// BLASTing Bozworth never increases skill

			return DobjMonster != null && DobjMonster.Uid == 20 ? false : base.ShouldAllowSkillGains();
		}
	}
}
