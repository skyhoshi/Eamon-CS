
// AttackCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework.Commands;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IAttackCommand))]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (BlastSpell || ActorMonster.Weapon > 0)
			{
				// Attack Bozworth

				if (DobjMonster != null && DobjMonster.Uid == 20)
				{
					Globals.Engine.PrintEffectDesc(20);

					DobjMonster.SetInLimbo();

					Globals.RtEngine.CheckEnemies();

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
				}

				// Attack/BLAST backpack

				else if (DobjArtifact != null && DobjArtifact.Uid == 13)
				{
					PrintDontNeedTo();

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
				}
				else
				{
					base.PlayerExecute();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
