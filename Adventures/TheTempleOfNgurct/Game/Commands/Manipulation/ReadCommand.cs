
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void PlayerProcessEvents(long eventType)
		{
			// Book

			if (eventType == PpeAfterArtifactRead && gDobjArtifact.Uid == 61)
			{
				gDobjArtifact.SetInRoom(gActorRoom);

				gGameState.Ro = 58;

				gGameState.R2 = gGameState.Ro;

				NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
				{
					x.MoveMonsters = false;
				});

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			// Brown potion

			if (gDobjArtifact.Uid == 51)
			{
				gEngine.PrintEffectDesc(1);
			}

			// Yellow potion

			else if (gDobjArtifact.Uid == 53)
			{
				gEngine.PrintEffectDesc(2);
			}

			// Red/black potion, fireball wand

			else if (gDobjArtifact.Uid == 52 || gDobjArtifact.Uid == 62 || gDobjArtifact.Uid == 63)
			{
				gEngine.PrintEffectDesc(3);
			}

			// Wine

			else if (gDobjArtifact.Uid == 69)
			{
				gEngine.PrintEffectDesc(4);
			}

			// Ring

			else if (gDobjArtifact.Uid == 64)
			{
				gEngine.PrintEffectDesc(5);
			}
			else
			{
				base.PlayerExecute();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public ReadCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
