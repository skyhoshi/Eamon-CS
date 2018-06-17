
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void PrintFortuneCookie()
		{
			Globals.Out.Print("The air crackles with magical energy but nothing interesting happens.");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				var cauldronArtifact = Globals.ADB[24];

				Debug.Assert(cauldronArtifact != null);

				// If the cauldron is prepared (see Effect #50) and the magic words have been spoken, unlock the portcullis

				if (ActorRoom.Uid == 43 && gameState.UsedCauldron && (cauldronArtifact.IsCarriedByCharacter() || cauldronArtifact.IsInRoom(ActorRoom)) && Globals.Engine.SpellReagentsInCauldron(cauldronArtifact))
				{
					Globals.Engine.PrintEffectDesc(52);

					// Unlock portcullis and destroy the cauldron

					gameState.UsedCauldron = false;

					var eastPortcullisArtifact = Globals.ADB[7];

					Debug.Assert(eastPortcullisArtifact != null);

					var ac = eastPortcullisArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac != null);

					ac.SetOpen(true);

					var westPortcullisArtifact = Globals.ADB[8];

					Debug.Assert(westPortcullisArtifact != null);

					ac = westPortcullisArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac != null);

					ac.SetOpen(true);

					Globals.Engine.RemoveWeight(cauldronArtifact);

					cauldronArtifact.SetInLimbo();

					Globals.Out.Print("The cauldron disintegrates!");

					GotoCleanup = true;

					goto Cleanup;
				}

				// Move companions into pit

				if (ActorRoom.Uid > 93 && ActorRoom.Uid < 110)
				{
					var monsters = Globals.Engine.GetMonsterList(() => true, m => !m.IsCharacterMonster() && m.Friendliness == Enums.Friendliness.Friend && m.Seen && (m.Location < 94 || m.Location > 109));

					if (monsters.Count > 0)
					{
						Globals.Engine.PrintEffectDesc(49);

						foreach (var m in monsters)
						{
							Globals.Out.Print("{0} suddenly appears!", m.GetDecoratedName03(true, true, false, false, Globals.Buf));

							m.SetInRoom(ActorRoom);
						}

						Globals.Engine.CheckEnemies();

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				// Move companions out of pit

				if (ActorRoom.Uid < 94 || ActorRoom.Uid > 109)
				{
					var monsters = Globals.Engine.GetMonsterList(() => true, m => !m.IsCharacterMonster() && m.Friendliness == Enums.Friendliness.Friend && m.Seen && (m.Location > 93 && m.Location < 110));

					if (monsters.Count > 0)
					{
						Globals.Engine.PrintEffectDesc(49);

						foreach (var m in monsters)
						{
							Globals.Out.Print("{0} suddenly appears!", m.GetDecoratedName03(true, true, false, false, Globals.Buf));

							m.SetInRoom(ActorRoom);
						}

						Globals.Engine.CheckEnemies();

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
