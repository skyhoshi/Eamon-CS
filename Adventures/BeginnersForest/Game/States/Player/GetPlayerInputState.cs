
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		protected override void ProcessEvents()
		{
			if (ShouldPreTurnProcess())
			{
				var gameState = Globals.GameState as Framework.IGameState;

				Debug.Assert(gameState != null);

				var fairyQueenMonster = Globals.MDB[8];

				Debug.Assert(fairyQueenMonster != null);

				// Meet Fairy Queen

				if (fairyQueenMonster.Location == gameState.Ro && fairyQueenMonster.Field1 == 0)
				{
					Globals.Engine.PrintEffectDesc(gameState.QueenGiftEffectUid);

					var crownArtifact = Globals.ADB[gameState.QueenGiftArtifactUid];

					Debug.Assert(crownArtifact != null);

					crownArtifact.SetInRoomUid(gameState.Ro);

					var grassBladeArtifact = Globals.ADB[6];

					Debug.Assert(grassBladeArtifact != null);

					// Grass blade turns into Greenblade

					if (grassBladeArtifact.IsCarriedByCharacter() || grassBladeArtifact.IsInRoomUid(gameState.Ro))
					{
						Globals.Engine.PrintEffectDesc(20);

						grassBladeArtifact.SetInLimbo();

						var greenBladeArtifact = Globals.ADB[8];

						Debug.Assert(greenBladeArtifact != null);

						greenBladeArtifact.SetInRoomUid(gameState.Ro);
					}

					fairyQueenMonster.Field1 = 1;
				}
			}

			base.ProcessEvents();
		}
	}
}
