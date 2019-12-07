
// LookCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			ActorRoom.Seen = false;

			if (Globals.GameState.GetNBTL(Friendliness.Enemy) <= 0)
			{
				var rl = Globals.Engine.RollDice(1, 100, 0);

				var room1 = Globals.RDB[24];

				Debug.Assert(room1 != null);

				var secretDoorArtifact1 = Globals.ADB[83];

				Debug.Assert(secretDoorArtifact1 != null);

				// Secret door

				if (ActorRoom.Uid == 24 && secretDoorArtifact1.IsInLimbo() && rl < 66)
				{
					secretDoorArtifact1.SetEmbeddedInRoom(ActorRoom);

					room1.SetDirectionDoor(Direction.North, secretDoorArtifact1);

					Globals.Engine.RevealEmbeddedArtifact(ActorRoom, secretDoorArtifact1);
				}

				var room2 = Globals.RDB[48];

				Debug.Assert(room2 != null);

				var secretDoorArtifact2 = Globals.ADB[84];

				Debug.Assert(secretDoorArtifact2 != null);

				// Secret door

				if (ActorRoom.Uid == 48 && secretDoorArtifact2.IsInLimbo() && rl < 51)
				{
					secretDoorArtifact2.SetEmbeddedInRoom(ActorRoom);

					room2.SetDirectionDoor(Direction.South, secretDoorArtifact2);

					Globals.Engine.RevealEmbeddedArtifact(ActorRoom, secretDoorArtifact2);
				}

				var scarabArtifact = Globals.ADB[65];

				Debug.Assert(scarabArtifact != null);

				// Hidden scarab

				if (ActorRoom.Uid == 38 && scarabArtifact.IsInLimbo())
				{
					scarabArtifact.SetInRoom(ActorRoom);
				}

				var guardMonster = Globals.MDB[30];

				Debug.Assert(guardMonster != null);

				var keyRingArtifact = Globals.ADB[72];

				Debug.Assert(keyRingArtifact != null);

				// Hidden ring of keys

				if (ActorRoom.Uid == gameState.KeyRingRoomUid && keyRingArtifact.IsInLimbo())
				{
					keyRingArtifact.SetInRoom(ActorRoom);

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
