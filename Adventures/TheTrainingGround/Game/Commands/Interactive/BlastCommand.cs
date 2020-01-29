
// BlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// BLAST Bozworth

			if (eventType == PpeAfterPlayerSpellCastCheck && gDobjMonster != null && gDobjMonster.Uid == 20)
			{
				gEngine.PrintEffectDesc(21);

				gDobjMonster.SetInLimbo();

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			// BLASTing Bozworth never increases skill

			return gDobjMonster != null && gDobjMonster.Uid == 20 ? false : base.ShouldAllowSkillGains();
		}
	}
}
