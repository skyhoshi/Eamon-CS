
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using BeginnersForest.Framework;
using BeginnersForest.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IGetPlayerInputState))]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		protected override void ProcessEvents()
		{
			if (ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as IGameState;

				Debug.Assert(gameState != null);

				var monster = Globals.MDB[8];

				Debug.Assert(monster != null);

				// Meet Fairy Queen

				if (monster.Location == gameState.Ro && monster.Field1 == 0)
				{
					Globals.Engine.PrintEffectDesc(gameState.QueenGiftEffectUid);

					var artifact = Globals.ADB[gameState.QueenGiftArtifactUid];

					Debug.Assert(artifact != null);

					artifact.SetInRoomUid(gameState.Ro);

					artifact = Globals.ADB[6];

					Debug.Assert(artifact != null);

					// Grass blade turns into Greenblade

					if (artifact.IsCarriedByCharacter() || artifact.IsInRoomUid(gameState.Ro))
					{
						Globals.Engine.PrintEffectDesc(20);

						artifact.SetInLimbo();

						artifact = Globals.ADB[8];

						Debug.Assert(artifact != null);

						artifact.SetInRoomUid(gameState.Ro);
					}

					monster.Field1 = 1;
				}
			}

			base.ProcessEvents();
		}
	}
}
