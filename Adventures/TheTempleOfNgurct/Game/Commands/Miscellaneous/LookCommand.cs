
// LookCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, EamonRT.Framework.Commands.ILookCommand
	{
		protected override void PlayerExecute()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			ActorRoom.Seen = false;

			if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0)
			{
				var rl = Globals.Engine.RollDice01(1, 100, 0);

				// Secret door

				if (ActorRoom.Uid == 24 && rl < 66)
				{
					var secretDoorArtifact = Globals.ADB[83];

					Debug.Assert(secretDoorArtifact != null);

					Globals.Engine.RevealEmbeddedArtifact(ActorRoom, secretDoorArtifact);
				}

				// Secret door

				if (ActorRoom.Uid == 48 && rl < 51)
				{
					var secretDoorArtifact = Globals.ADB[84];

					Debug.Assert(secretDoorArtifact != null);

					Globals.Engine.RevealEmbeddedArtifact(ActorRoom, secretDoorArtifact);
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
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
		}
	}
}
