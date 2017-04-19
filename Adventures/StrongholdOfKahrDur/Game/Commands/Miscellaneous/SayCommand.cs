
// SayCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using StrongholdOfKahrDur.Framework;
using StrongholdOfKahrDur.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ISayCommand))]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		protected override void PlayerProcessEvents01()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			// Restore monster stats to average for testing/debugging

			if (string.Equals(ProcessedPhrase, "*brutis", StringComparison.OrdinalIgnoreCase))
			{
				var artUid = ActorMonster.Weapon;

				ActorMonster.Weapon = -1;

				Globals.RtEngine.InitMonsterScaledHardinessValues();

				ActorMonster.Weapon = artUid;

				Globals.RtEngine.CheckEnemies();

				Globals.Out.WriteLine("{0}Monster stats reduced.", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				goto Cleanup;
			}

			var artifact = Globals.ADB[24];

			Debug.Assert(artifact != null);

			// If the cauldron is present and the spell components (see effect #50) are in it then begin the spell casting process

			if (string.Equals(ProcessedPhrase, "knock nikto mellon", StringComparison.OrdinalIgnoreCase) && (artifact.IsCarriedByCharacter() || artifact.IsInRoom(ActorRoom)) && Globals.RtEngine.CastTo<IRtEngine>().SpellReagentsInCauldron(artifact))
			{
				Globals.Engine.PrintEffectDesc(51);

				gameState.UsedCauldron = true;
			}

			var monster = Globals.MDB[15];

			Debug.Assert(monster != null);

			// Player will agree to free the Lich

			if (string.Equals(ProcessedPhrase, "i will free you", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 109 && monster.IsInRoom(ActorRoom) && monster.Friendliness > Enums.Friendliness.Enemy && gameState.LichState < 2)
			{
				Globals.Engine.PrintEffectDesc(54);

				gameState.LichState = 1;
			}

			// Player actually frees the Lich

			if (string.Equals(ProcessedPhrase, "barada lhain", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 109 && monster.IsInRoom(ActorRoom) && gameState.LichState == 1)
			{
				var artifact01 = Globals.ADB[25];

				Debug.Assert(artifact01 != null);

				Globals.Engine.PrintEffectDesc(55);

				// Set freed Lich flag and give Wizard's Helm (25) to player (carried but not worn)

				gameState.LichState = 2;

				artifact01.SetInRoom(ActorRoom);
			}

			base.PlayerProcessEvents01();

		Cleanup:

			;
		}
	}
}
