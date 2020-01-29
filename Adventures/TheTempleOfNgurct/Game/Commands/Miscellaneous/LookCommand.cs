
// LookCommand.cs

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
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{
		public override void PlayerExecute()
		{
			gActorRoom.Seen = false;

			if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				var room1 = gRDB[24];

				Debug.Assert(room1 != null);

				var secretDoorArtifact1 = gADB[83];

				Debug.Assert(secretDoorArtifact1 != null);

				// Secret door

				if (gActorRoom.Uid == 24 && secretDoorArtifact1.IsInLimbo() && rl < 66)
				{
					secretDoorArtifact1.SetEmbeddedInRoom(gActorRoom);

					room1.SetDirectionDoor(Direction.North, secretDoorArtifact1);

					gEngine.RevealEmbeddedArtifact(gActorRoom, secretDoorArtifact1);
				}

				var room2 = gRDB[48];

				Debug.Assert(room2 != null);

				var secretDoorArtifact2 = gADB[84];

				Debug.Assert(secretDoorArtifact2 != null);

				// Secret door

				if (gActorRoom.Uid == 48 && secretDoorArtifact2.IsInLimbo() && rl < 51)
				{
					secretDoorArtifact2.SetEmbeddedInRoom(gActorRoom);

					room2.SetDirectionDoor(Direction.South, secretDoorArtifact2);

					gEngine.RevealEmbeddedArtifact(gActorRoom, secretDoorArtifact2);
				}

				var scarabArtifact = gADB[65];

				Debug.Assert(scarabArtifact != null);

				// Hidden scarab

				if (gActorRoom.Uid == 38 && scarabArtifact.IsInLimbo())
				{
					scarabArtifact.SetInRoom(gActorRoom);
				}

				var guardMonster = gMDB[30];

				Debug.Assert(guardMonster != null);

				var keyRingArtifact = gADB[72];

				Debug.Assert(keyRingArtifact != null);

				// Hidden ring of keys

				if (gActorRoom.Uid == gGameState.KeyRingRoomUid && keyRingArtifact.IsInLimbo())
				{
					keyRingArtifact.SetInRoom(gActorRoom);

					// Guard wasn't killed

					if (guardMonster.DmgTaken < guardMonster.Hardiness)
					{
						keyRingArtifact.Desc = "You find a ring of keys lying abandoned nearby.";
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}
	}
}
