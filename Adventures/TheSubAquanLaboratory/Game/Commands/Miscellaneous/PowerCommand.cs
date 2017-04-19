
// PowerCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPowerCommand))]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected virtual bool IsActorRoomInLab()
		{
			return ActorRoom.Uid == 18 || ActorRoom.Zone == 2;
		}

		protected override void PrintSonicBoom()
		{
			Globals.Engine.PrintEffectDesc(80 + (IsActorRoomInLab() ? 1 : 0));
		}

		protected override void PlayerProcessEvents()
		{
			var rl = 0L;

			var rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (rl < 11)
			{
				// 10% Chance of raising the dead

				var found = false;

				var artifacts = Globals.Engine.GetArtifactList(() => true, a => (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)) && a.IsDeadBody());

				foreach (var artifact in artifacts)
				{
					var monster = Globals.Database.MonsterTable.Records.FirstOrDefault(m => m.DeadBody == artifact.Uid);

					Debug.Assert(monster != null);

					if (monster.OrigGroupCount == 1)
					{
						monster.SetInRoom(ActorRoom);

						monster.DmgTaken = 0;

						Globals.RtEngine.RemoveWeight(artifact);

						artifact.SetInLimbo();

						Globals.Out.Write("{0}{1} comes to life!{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));

						found = true;
					}
				}

				if (found)
				{
					Globals.RtEngine.CheckEnemies();

					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 21)
			{
				// 10% Chance of stuff vanishing

				var artifacts = Globals.Engine.GetArtifactList(() => true, a => a.IsInRoom(ActorRoom) && !a.IsUnmovable() && a.Uid != 82 && a.Uid != 89);

				foreach (var artifact in artifacts)
				{
					artifact.SetInLimbo();

					Globals.Out.Write("{0}{1} vanishes!{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
				}

				if (artifacts.Count > 0)
				{
					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 31)
			{
				// 10% Chance of cracking dome

				if (IsActorRoomInLab())
				{
					Globals.Engine.PrintEffectDesc(44);

					Globals.GameState.Die = 1;

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 41)
			{
				// 10% Chance of insta-heal (tm)

				var monster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(monster != null);

				if (monster.DmgTaken > 0)
				{
					monster.DmgTaken = 0;

					Globals.RtEngine.CheckEnemies();

					Globals.Out.Write("{0}All of your wounds are suddenly healed!{0}", Environment.NewLine);

					Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

					ActorMonster.AddHealthStatus(Globals.Buf);

					Globals.Out.Write("{0}", Globals.Buf);

					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 101)
			{
				// 60% Chance of boom over lake/in lab or fortune cookie

				base.PlayerProcessEvents();
			}

		Cleanup:

			;
		}
	}
}
