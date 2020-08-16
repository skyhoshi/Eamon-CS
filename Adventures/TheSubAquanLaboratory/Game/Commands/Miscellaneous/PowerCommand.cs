
// PowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void PlayerProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterPlayerSpellCastCheck)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				if (rl < 11)
				{
					// 10% Chance of raising the dead

					var found = gEngine.ResurrectDeadBodies(ActorRoom);

					if (found)
					{
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

					var found = gEngine.MakeArtifactsVanish(ActorRoom, a => a.IsInRoom(ActorRoom) && !a.IsUnmovable() && a.Uid != 82 && a.Uid != 89);

					if (found)
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
						gEngine.PrintEffectDesc(44);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
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

					if (ActorMonster.DmgTaken > 0)
					{
						ActorMonster.DmgTaken = 0;

						gOut.Print("All of your wounds are suddenly healed!");

						Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

						ActorMonster.AddHealthStatus(Globals.Buf);

						gOut.Write("{0}", Globals.Buf);

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

					base.PlayerProcessEvents(eventType);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}

		Cleanup:

			;
		}

		public override void PrintSonicBoom()
		{
			gEngine.PrintEffectDesc(80 + (IsActorRoomInLab() ? 1 : 0));
		}

		public virtual bool IsActorRoomInLab()
		{
			return ActorRoom.Uid == 18 || ActorRoom.Zone == 2;
		}
	}
}
