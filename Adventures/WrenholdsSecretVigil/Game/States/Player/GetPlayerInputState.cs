
// GetPlayerInputState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework;
using WrenholdsSecretVigil.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IGetPlayerInputState))]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		protected override void ProcessEvents()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

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
					Globals.Out.Write("{0}All sides of the magic cube are glowing!{0}", Environment.NewLine);
				}
				else
				{
					var dir = "south";

					if (gameState.Ro == 67 || gameState.Ro == 68)
					{
						dir = "west";
					}
					else if (characterRoom.GetDirs(Enums.Direction.East) > 0 && characterRoom.GetDirs(Enums.Direction.East) <= Globals.Module.NumRooms)
					{
						dir = "east";
					}

					Globals.Out.Write("{0}The {1} side of the magic cube is glowing!{0}", Environment.NewLine, dir);
				}
			}

			base.ProcessEvents();
		}
	}
}
