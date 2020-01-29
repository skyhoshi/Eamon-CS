
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterEnforceMonsterWeightLimitsCheck)
			{
				// Give Prince the Runcible Cargo

				if ((gIobjMonster.Uid == 38 || gIobjMonster.Uid == 39) && gDobjArtifact.Uid == 129)
				{
					gDobjArtifact.SetCarriedByMonsterUid(38);

					gCharacter.HeldGold += 2000;

					gEngine.PrintEffectDesc(132);

					GotoCleanup = true;
				}

				// Give Bandit's Guild Commander the Runcible Cargo

				else if ((gIobjMonster.Uid == 27 || gIobjMonster.Uid == 28) && gDobjArtifact.Uid == 129)
				{
					gEngine.PrintEffectDesc(131);

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;
				}

				// Give Larkspur his pills

				else if (gIobjMonster.Uid == 36 && gDobjArtifact.Uid == 130)
				{
					gDobjArtifact.SetInLimbo();

					gIobjMonster.Friendliness++;

					gIobjMonster.OrigFriendliness = (Friendliness)200;

					gEngine.PrintEffectDesc(94);

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (gIobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterSmiles(gIobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}

				// Nobody wants to hold the Runcible Cargo

				else if (gDobjArtifact.Uid == 129)
				{
					gOut.Print("{0} nervously refuse{1} your offer.",
						gIobjMonster.GetTheName(true),
						gIobjMonster.EvalPlural("s", ""));

					GotoCleanup = true;
				}
				else
				{
					base.PlayerProcessEvents(eventType);
				}
			}
			else if (eventType == PpeBeforeMonsterTakesGold)
			{
				// Give $ to Amazon

				if (gIobjMonster.Uid == 22 && gIobjMonster.Friendliness == Friendliness.Friend)
				{
					var gender = Math.Min((long)gActorMonster.Gender, 1);

					gEngine.PrintEffectDesc(153 + gGameState.GiveAmazonMoney + gender * 2);

					gGameState.GiveAmazonMoney = 1;

					GotoCleanup = true;
				}

				// Disable bribing

				else if (gIobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterSmiles(gIobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
				else
				{
					base.PlayerProcessEvents(eventType);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
