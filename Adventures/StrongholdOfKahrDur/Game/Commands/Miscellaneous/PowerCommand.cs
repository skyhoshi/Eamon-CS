
// PowerCommand.cs

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
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPowerCommand))]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected override void PrintFortuneCookie()
		{
			Globals.Out.Write("{0}The air crackles with magical energy but nothing interesting happens.{0}", Environment.NewLine);
		}

		protected override void PlayerProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			var artifact = Globals.ADB[24];

			Debug.Assert(artifact != null);

			// If the cauldron is prepared (see Effect #50) and the magic words have been spoken, unlock the portcullis

			if (ActorRoom.Uid == 43 && gameState.UsedCauldron && (artifact.IsCarriedByCharacter() || artifact.IsInRoom(ActorRoom)) && Globals.RtEngine.CastTo<IRtEngine>().SpellReagentsInCauldron(artifact))
			{
				Globals.Engine.PrintEffectDesc(52);

				// Unlock portcullis and destroy the cauldron

				gameState.UsedCauldron = false;

				var artifact01 = Globals.ADB[7];

				Debug.Assert(artifact01 != null);

				var ac = artifact01.GetArtifactClass(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				ac.SetOpen(true);

				artifact01 = Globals.ADB[8];

				Debug.Assert(artifact01 != null);

				ac = artifact01.GetArtifactClass(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				ac.SetOpen(true);

				Globals.RtEngine.RemoveWeight(artifact);

				artifact.SetInLimbo();

				Globals.Out.WriteLine("{0}The cauldron disintegrates!", Environment.NewLine);

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
						Globals.Out.WriteLine("{0}{1} suddenly appears!", Environment.NewLine, m.GetDecoratedName03(true, true, false, false, Globals.Buf));

						m.SetInRoom(ActorRoom);
					}

					Globals.RtEngine.CheckEnemies();

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
						Globals.Out.WriteLine("{0}{1} suddenly appears!", Environment.NewLine, m.GetDecoratedName03(true, true, false, false, Globals.Buf));

						m.SetInRoom(ActorRoom);
					}

					Globals.RtEngine.CheckEnemies();

					GotoCleanup = true;

					goto Cleanup;
				}
			}

			base.PlayerProcessEvents();

		Cleanup:

			;
		}
	}
}
