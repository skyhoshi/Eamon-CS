
// GetCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGetCommand))]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		protected override void PrintReceived(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// Request medallion

			if (artifact.Uid == 10 && gameState.MedallionCharges > 0)
			{
				base.PrintReceived(artifact);

				Globals.Out.Write("{0}{0}Your hand feels relaxed, but strong.", Environment.NewLine);
			}
			else
			{
				base.PrintReceived(artifact);
			}
		}

		protected override void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// Remove orb

			if (artifact.Uid == 4 && !gameState.RemovedLifeOrb)
			{
				Globals.Engine.PrintEffectDesc(33);

				Globals.Engine.PrintEffectDesc(34);

				gameState.RemovedLifeOrb = true;

				base.PrintRetrieved(artifact);
			}

			// Remove medallion

			else if (artifact.Uid == 10 && gameState.MedallionCharges > 0)
			{
				base.PrintRetrieved(artifact);

				Globals.Out.Write("{0}{0}Your hand feels relaxed, but strong.", Environment.NewLine);
			}
			else
			{
				base.PrintRetrieved(artifact);
			}
		}

		protected override void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			var bronzeKeyArtifact = Globals.ADB[27];

			Debug.Assert(bronzeKeyArtifact != null);

			var deviceArtifact1 = Globals.ADB[44];

			Debug.Assert(deviceArtifact1 != null);

			var deviceArtifact2 = Globals.ADB[49];

			Debug.Assert(deviceArtifact2 != null);

			var ac = artifact.Classes[0];

			// Get lever

			if (artifact.Uid == 48 && deviceArtifact1.IsInRoom(ActorRoom))
			{
				if (ArtifactList[0] != artifact)
				{
					Globals.Out.WriteLine();
				}

				deviceArtifact1.SetInLimbo();

				deviceArtifact2.SetInRoom(ActorRoom);

				Globals.Engine.PrintEffectDesc(30);

				base.PrintTaken(artifact);
			}

			// Get medallion

			else if (artifact.Uid == 10 && gameState.MedallionCharges > 0)
			{
				base.PrintTaken(artifact);

				Globals.Out.Write("{0}{0}Your hand feels relaxed, but strong.", Environment.NewLine);

				if (ArtifactList[ArtifactList.Count - 1] != artifact)
				{
					Globals.Out.WriteLine();
				}
			}

			// Get straw mattress

			else if (artifact.Uid == 26 && bronzeKeyArtifact.IsInLimbo())
			{
				base.PrintTaken(artifact);

				bronzeKeyArtifact.SetInRoom(ActorRoom);

				Globals.Out.Write("{0}{0}You found something under the mattress!", Environment.NewLine);

				if (ArtifactList[ArtifactList.Count - 1] != artifact)
				{
					Globals.Out.WriteLine();
				}
			}

			// Get gold curtain

			else if (artifact.Uid == 40 && ac.Type == Enums.ArtifactType.DoorGate)
			{
				base.PrintTaken(artifact);

				ActorRoom.SetDirs(Enums.Direction.South, 68);

				ac.Type = Enums.ArtifactType.Treasure;

				ac.Field5 = 0;

				ac.Field6 = 0;

				ac.Field7 = 0;

				ac.Field8 = 0;
			}
			else
			{
				base.PrintTaken(artifact);
			}
		}

		protected virtual void PrintCantGetSlime()
		{
			Globals.Out.Write("{0}Corrosive slime is not something to get.{0}", Environment.NewLine);
		}

		protected virtual void PrintCantDetachRope()
		{
			Globals.Out.Write("{0}You cannot detach the rope.{0}", Environment.NewLine);
		}

		protected override void ProcessArtifact(IArtifact artifact, IArtifactClass ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// Get slime

			if (artifact.Uid == 24 || artifact.Uid == 25)
			{
				ProcessAction(() => PrintCantGetSlime(), ref nlFlag);
			}

			// Get rope

			else if (artifact.Uid == 28)
			{
				Globals.Engine.PrintEffectDesc(25);

				if (!gameState.PulledRope)
				{
					var monsters = Globals.Engine.GetMonsterList(() => true, m => m.Uid >= 14 && m.Uid <= 16);

					foreach (var monster in monsters)
					{
						monster.SetInRoomUid(48);
					}

					// CheckEnemies intentionally omitted

					gameState.PulledRope = true;
				}

				ProcessAction(() => PrintCantDetachRope(), ref nlFlag);
			}
			else
			{
				base.ProcessArtifact(artifact, ac, ref nlFlag);
			}
		}
	}
}
