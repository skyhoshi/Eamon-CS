
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			if (BlastSpell || gActorMonster.Weapon > 0)
			{
				// Attack Bozworth

				if (gDobjMonster != null && gDobjMonster.Uid == 20)
				{
					gEngine.PrintEffectDesc(20);

					gDobjMonster.SetInLimbo();

					NextState = Globals.CreateInstance<IStartState>();
				}

				// Attack/BLAST backpack

				else if (gDobjArtifact != null && gDobjArtifact.Uid == 13)
				{
					PrintDontNeedTo();

					NextState = Globals.CreateInstance<IStartState>();
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
