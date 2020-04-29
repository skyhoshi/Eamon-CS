﻿
// UseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			// Digging with shovel

			if (gDobjArtifact.Uid == 7)
			{
				var buriedCasketArtifact = gADB[35];

				Debug.Assert(buriedCasketArtifact != null);

				if (gActorRoom.Uid == 5 && buriedCasketArtifact.IsInLimbo())
				{
					gOut.Print("You dig for a while and uncover something!");

					buriedCasketArtifact.SetInRoom(gActorRoom);
				}
				else
				{
					var digResult = gActorRoom.EvalRoomType("The floor is far to hard to dig into!", "You dig for a while but find nothing of interest.");

					gOut.Print(digResult);
				}

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// Bailing fountain water with wooden bucket

			else if (gDobjArtifact.Uid == 6)
			{
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				var largeFountainArtifact = gADB[24];

				Debug.Assert(largeFountainArtifact != null);

				var waterArtifact = gADB[40];

				Debug.Assert(waterArtifact != null);

				if (gActorRoom.Uid == 116 && !waterArtifact.IsInLimbo())
				{
					if (waterWeirdMonster.IsInRoom(gActorRoom))
					{
						gOut.Print("{0} won't let you get close enough to do that!", waterWeirdMonster.GetTheName(true));
					}
					else if (!gGameState.WaterWeirdKilled)
					{
						gEngine.PrintEffectDesc(100);

						waterWeirdMonster.SetInRoom(gActorRoom);

						NextState = Globals.CreateInstance<IStartState>();
					}
					else
					{
						gOut.Print("You remove all the water from the fountain.");

						waterArtifact.SetInLimbo();

						largeFountainArtifact.Desc = largeFountainArtifact.Desc.Replace("squirts", "squirted");
					}
				}
				else
				{
					gOut.Print("That doesn't do anything right now.");
				}

				if (NextState == null)
				{
					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}

			// Using bronze cross on undead

			else if (gDobjArtifact.Uid == 37)
			{
				var deadMonsterUids = new long[] { 3, 4, 6 };
				
				var wardedMonsterUids = new long[] { 7, 8, 9, 14, 16 };

				var deadMonsterList = gEngine.GetMonsterList(m => deadMonsterUids.Contains(m.Uid) && m.IsInRoom(gActorRoom));

				foreach(var m in deadMonsterList)
				{
					gOut.Print("{0} is destroyed by the cross, its body disintegrates!", m.GetTheName(true));

					m.SetInLimbo();

					m.DmgTaken = 0;
				}

				var wardedMonsterList = gEngine.GetMonsterList(m => wardedMonsterUids.Contains(m.Uid) && m.IsInRoom(gActorRoom));

				foreach (var m in wardedMonsterList)
				{
					gOut.Print("{0} is warded off by the cross!", m.GetTheName(true));

					gEngine.MoveMonsterToRandomAdjacentRoom(gActorRoom, m, true, false);
				}

				if (deadMonsterList.Count == 0 && wardedMonsterList.Count == 0)
				{
					gOut.Print("Nothing happens.");
				}

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}