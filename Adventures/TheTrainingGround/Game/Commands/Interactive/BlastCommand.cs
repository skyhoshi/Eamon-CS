
// BlastCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using TheTrainingGround.Framework.Commands;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IBlastCommand))]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		protected override void PlayerProcessEvents()
		{
			// BLAST Bozworth

			if (DobjMonster != null && DobjMonster.Uid == 20)
			{
				Globals.Engine.PrintEffectDesc(21);

				DobjMonster.SetInLimbo();

				Globals.RtEngine.CheckEnemies();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected override bool AllowSkillIncrease()
		{
			// BLASTing Bozworth never increases skill

			return DobjMonster != null && DobjMonster.Uid == 20 ? false : base.AllowSkillIncrease();
		}
	}
}
