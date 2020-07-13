﻿
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

				if ((IobjMonster.Uid == 38 || IobjMonster.Uid == 39) && DobjArtifact.Uid == 129)
				{
					DobjArtifact.SetCarriedByMonsterUid(38);

					gCharacter.HeldGold += 2000;

					gEngine.PrintEffectDesc(132);

					GotoCleanup = true;
				}

				// Give Bandit's Guild Commander the Runcible Cargo

				else if ((IobjMonster.Uid == 27 || IobjMonster.Uid == 28) && DobjArtifact.Uid == 129)
				{
					gEngine.PrintEffectDesc(131);

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;
				}

				// Give Larkspur his pills

				else if (IobjMonster.Uid == 36 && DobjArtifact.Uid == 130)
				{
					DobjArtifact.SetInLimbo();

					IobjMonster.Friendliness++;

					IobjMonster.OrigFriendliness = (Friendliness)200;

					gEngine.PrintEffectDesc(94);

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (IobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}

				// Nobody wants to hold the Runcible Cargo

				else if (DobjArtifact.Uid == 129)
				{
					gOut.Print("{0} nervously refuse{1} your offer.",
						IobjMonster.GetTheName(true),
						IobjMonster.EvalPlural("s", ""));

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

				if (IobjMonster.Uid == 22 && IobjMonster.Friendliness == Friendliness.Friend)
				{
					var gender = Math.Min((long)ActorMonster.Gender, 1);

					gEngine.PrintEffectDesc(153 + gGameState.GiveAmazonMoney + gender * 2);

					gGameState.GiveAmazonMoney = 1;

					GotoCleanup = true;
				}

				// Disable bribing

				else if (IobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterEmotes(IobjMonster);

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
