
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PpeAfterPlayerSay)
			{
				// Restore monster stats to average for testing/debugging

				if (string.Equals(ProcessedPhrase, "*brutis", StringComparison.OrdinalIgnoreCase))
				{
					var artUid = ActorMonster.Weapon;

					ActorMonster.Weapon = -1;

					Globals.Engine.InitMonsterScaledHardinessValues();

					ActorMonster.Weapon = artUid;

					Globals.Out.Print("Monster stats reduced.");

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				var cauldronArtifact = Globals.ADB[24];

				Debug.Assert(cauldronArtifact != null);

				// If the cauldron is present and the spell components (see effect #50) are in it then begin the spell casting process

				if (string.Equals(ProcessedPhrase, "knock nikto mellon", StringComparison.OrdinalIgnoreCase) && (cauldronArtifact.IsCarriedByCharacter() || cauldronArtifact.IsInRoom(ActorRoom)) && Globals.Engine.SpellReagentsInCauldron(cauldronArtifact))
				{
					Globals.Engine.PrintEffectDesc(51);

					gameState.UsedCauldron = true;
				}

				var lichMonster = Globals.MDB[15];

				Debug.Assert(lichMonster != null);

				// Player will agree to free the Lich

				if (string.Equals(ProcessedPhrase, "i will free you", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 109 && lichMonster.IsInRoom(ActorRoom) && lichMonster.Friendliness > Friendliness.Enemy && gameState.LichState < 2)
				{
					Globals.Engine.PrintEffectDesc(54);

					gameState.LichState = 1;
				}

				// Player actually frees the Lich

				if (string.Equals(ProcessedPhrase, "barada lhain", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 109 && lichMonster.IsInRoom(ActorRoom) && gameState.LichState == 1)
				{
					var helmArtifact = Globals.ADB[25];

					Debug.Assert(helmArtifact != null);

					Globals.Engine.PrintEffectDesc(55);

					// Set freed Lich flag and give Wizard's Helm (25) to player (carried but not worn)

					gameState.LichState = 2;

					helmArtifact.SetInRoom(ActorRoom);
				}
			}

			base.PlayerProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
