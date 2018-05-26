
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		protected override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PpeAfterEnforceMonsterWeightLimitsCheck)
			{
				// Give Prince the Runcible Cargo

				if ((IobjMonster.Uid == 38 || IobjMonster.Uid == 39) && DobjArtifact.Uid == 129)
				{
					Globals.Engine.RemoveWeight(DobjArtifact);

					DobjArtifact.SetCarriedByMonsterUid(38);

					Globals.Character.HeldGold += 2000;

					Globals.Engine.PrintEffectDesc(132);

					GotoCleanup = true;
				}

				// Give Bandit's Guild Commander the Runcible Cargo

				else if ((IobjMonster.Uid == 27 || IobjMonster.Uid == 28) && DobjArtifact.Uid == 129)
				{
					Globals.Engine.PrintEffectDesc(131);

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;
				}

				// Give Larkspur his pills

				else if (IobjMonster.Uid == 36 && DobjArtifact.Uid == 130)
				{
					Globals.Engine.RemoveWeight(DobjArtifact);

					DobjArtifact.SetInLimbo();

					IobjMonster.Friendliness++;

					IobjMonster.OrigFriendliness = (Enums.Friendliness)200;

					Globals.Engine.CheckEnemies();

					Globals.Engine.PrintEffectDesc(94);

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (IobjMonster.Friendliness < Enums.Friendliness.Friend)
				{
					Globals.Engine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();

					GotoCleanup = true;
				}

				// Nobody wants to hold the Runcible Cargo

				else if (DobjArtifact.Uid == 129)
				{
					Globals.Out.Print("{0} nervously refuse{1} your offer.",
						IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf),
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

				if (IobjMonster.Uid == 22 && IobjMonster.Friendliness == Enums.Friendliness.Friend)
				{
					var gender = Math.Min((long)ActorMonster.Gender, 1);

					Globals.Engine.PrintEffectDesc(153 + gameState.GiveAmazonMoney + gender * 2);

					gameState.GiveAmazonMoney = 1;

					GotoCleanup = true;
				}

				// Disable bribing

				else if (IobjMonster.Friendliness < Enums.Friendliness.Friend)
				{
					Globals.Engine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();

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

		protected override bool MonsterRefusesToAccept()
		{
			return false;
		}
	}
}
