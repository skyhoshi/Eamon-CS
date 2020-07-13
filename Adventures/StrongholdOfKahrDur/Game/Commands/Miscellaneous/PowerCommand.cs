﻿
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void PrintFortuneCookie()
		{
			gOut.Print("The air crackles with magical energy but nothing interesting happens.");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				var cauldronArtifact = gADB[24];

				Debug.Assert(cauldronArtifact != null);

				// If the cauldron is prepared (see Effect #50) and the magic words have been spoken, unlock the portcullis

				if (ActorRoom.Uid == 43 && gGameState.UsedCauldron && (cauldronArtifact.IsCarriedByCharacter() || cauldronArtifact.IsInRoom(ActorRoom)) && gEngine.SpellReagentsInCauldron(cauldronArtifact))
				{
					gEngine.PrintEffectDesc(52);

					// Unlock portcullis and destroy the cauldron

					gGameState.UsedCauldron = false;

					var eastPortcullisArtifact = gADB[7];

					Debug.Assert(eastPortcullisArtifact != null);

					var ac = eastPortcullisArtifact.DoorGate;

					Debug.Assert(ac != null);

					ac.SetOpen(true);

					var westPortcullisArtifact = gADB[8];

					Debug.Assert(westPortcullisArtifact != null);

					ac = westPortcullisArtifact.DoorGate;

					Debug.Assert(ac != null);

					ac.SetOpen(true);

					cauldronArtifact.SetInLimbo();

					gOut.Print("The cauldron disintegrates!");

					GotoCleanup = true;

					goto Cleanup;
				}

				// Move companions into pit

				if (ActorRoom.Uid > 93 && ActorRoom.Uid < 110)
				{
					var monsters = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Friendliness == Friendliness.Friend && m.Seen && (m.Location < 94 || m.Location > 109));

					if (monsters.Count > 0)
					{
						gEngine.PrintEffectDesc(49);

						foreach (var m in monsters)
						{
							gOut.Print("{0} suddenly appears!", m.GetTheName(true));

							m.SetInRoom(ActorRoom);
						}

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				// Move companions out of pit

				if (ActorRoom.Uid < 94 || ActorRoom.Uid > 109)
				{
					var monsters = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Friendliness == Friendliness.Friend && m.Seen && (m.Location > 93 && m.Location < 110));

					if (monsters.Count > 0)
					{
						gEngine.PrintEffectDesc(49);

						foreach (var m in monsters)
						{
							gOut.Print("{0} suddenly appears!", m.GetTheName(true));

							m.SetInRoom(ActorRoom);
						}

						GotoCleanup = true;

						goto Cleanup;
					}
				}
			}

			base.PlayerProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
