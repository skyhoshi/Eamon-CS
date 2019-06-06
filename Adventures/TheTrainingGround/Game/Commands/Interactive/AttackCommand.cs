
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (BlastSpell || ActorMonster.Weapon > 0)
			{
				// Attack Bozworth

				if (DobjMonster != null && DobjMonster.Uid == 20)
				{
					Globals.Engine.PrintEffectDesc(20);

					DobjMonster.SetInLimbo();

					NextState = Globals.CreateInstance<IStartState>();
				}

				// Attack/BLAST backpack

				else if (DobjArtifact != null && DobjArtifact.Uid == 13)
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
