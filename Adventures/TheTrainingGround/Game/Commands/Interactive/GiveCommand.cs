
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, EamonRT.Framework.Commands.IGiveCommand
	{
		protected override void PrintGiveObjToActor(Eamon.Framework.IArtifact artifact, Eamon.Framework.IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			base.PrintGiveObjToActor(artifact, monster);

			// Give rapier to Jacques

			if (monster.Uid == 5 && artifact.Uid == 8 && !gameState.JacquesRecoversRapier)
			{
				Globals.Engine.PrintEffectDesc(22);

				gameState.JacquesRecoversRapier = true;
			}
		}

		protected override void PlayerProcessEvents()
		{
			// Give obsidian scroll case to Emerald Warrior

			if (IobjMonster.Uid == 14 && DobjArtifact.Uid == 51)
			{
				Globals.Engine.RemoveWeight(DobjArtifact);

				DobjArtifact.SetInLimbo();

				IobjMonster.SetInLimbo();

				Globals.Engine.CheckEnemies();

				Globals.Engine.PrintEffectDesc(14);

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

			// Buy potion from gnome

			if (IobjMonster.Uid == 20)
			{
				if (GoldAmount >= 100)
				{
					var redPotionArtifact = Globals.ADB[40];

					Debug.Assert(redPotionArtifact != null);

					var bluePotionArtifact = Globals.ADB[41];

					Debug.Assert(bluePotionArtifact != null);

					if (redPotionArtifact.IsCarriedByMonsterUid(20) || bluePotionArtifact.IsCarriedByMonsterUid(20))
					{
						Globals.Character.HeldGold -= GoldAmount;

						if (GoldAmount > 100)
						{
							Globals.Engine.PrintEffectDesc(30);
						}

						var potionArtifact = redPotionArtifact.IsCarriedByMonsterUid(20) ? redPotionArtifact : bluePotionArtifact;

						potionArtifact.SetInRoomUid(gameState.Ro);

						Globals.Engine.PrintEffectDesc(31);

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
					}
					else
					{
						Globals.Engine.PrintEffectDesc(29);
					}
				}
				else
				{
					Globals.Engine.PrintEffectDesc(28);
				}

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents02();
			}
		}

		protected override bool MonsterRefusesToAccept()
		{
			return ((IobjMonster.Uid == 5 && DobjArtifact.Uid == 8) || (IobjMonster.Uid == 14 && DobjArtifact.Uid == 51)) ? false : base.MonsterRefusesToAccept();
		}
	}
}
