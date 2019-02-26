
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeBeforeCommandPromptPrint)
			{
				var lifeOrbArtifact = Globals.ADB[4];

				Debug.Assert(lifeOrbArtifact != null);

				var magicCubeArtifact = Globals.ADB[5];

				Debug.Assert(magicCubeArtifact != null);

				// Magic cube code

				if (magicCubeArtifact.IsCarriedByCharacter() && gameState.Ro >= 40 && lifeOrbArtifact.IsCarriedByContainerUid(49))
				{
					var characterRoom = Globals.RDB[gameState.Ro];

					Debug.Assert(characterRoom != null);

					if (gameState.Ro == 69)
					{
						Globals.Out.Print("All sides of the magic cube are glowing!");
					}
					else
					{
						var dir = "south";

						if (gameState.Ro == 67 || gameState.Ro == 68)
						{
							dir = "west";
						}
						else if (characterRoom.GetDirs(Direction.East) > 0 && characterRoom.GetDirs(Direction.East) <= Globals.Module.NumRooms)
						{
							dir = "east";
						}

						Globals.Out.Print("The {0} side of the magic cube is glowing!", dir);
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
