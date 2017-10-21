
// GiveCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using ARuncibleCargo.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGiveCommand))]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		protected override void PlayerProcessEvents()
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

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

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
				Globals.Out.Write("{0}{1} nervously refuse{2} your offer.{0}",
					Environment.NewLine,
					IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf),
					IobjMonster.EvalPlural("s", ""));

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected override void PlayerProcessEvents02()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

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
				base.PlayerProcessEvents02();
			}
		}

		protected override bool MonsterRefusesToAccept()
		{
			return false;
		}
	}
}
