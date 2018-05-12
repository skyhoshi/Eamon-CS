
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.States
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

				var characterMonster = Globals.MDB[gameState.Cm];

				Debug.Assert(characterMonster != null);

				var characterRoom = characterMonster.GetInRoom();

				Debug.Assert(characterRoom != null);

				// Events only occur in lit rooms

				if (characterRoom.IsLit())
				{
					var redSunMonster = Globals.MDB[1];

					Debug.Assert(redSunMonster != null);

					// Red Sun speaks

					if (!gameState.RedSunSpeaks && redSunMonster.IsInRoom(characterRoom))
					{
						Globals.Engine.PrintEffectDesc(4);

						gameState.RedSunSpeaks = true;
					}

					// Jacques shouts from behind door

					if (!gameState.JacquesShouts && characterRoom.Uid == 8)
					{
						Globals.Engine.PrintEffectDesc(5);

						gameState.JacquesShouts = true;
					}

					var sylvaniMonster = Globals.MDB[12];

					Debug.Assert(sylvaniMonster != null);

					// Sylvani speaks

					if (!gameState.SylvaniSpeaks && sylvaniMonster.IsInRoom(characterRoom))
					{
						Globals.Engine.PrintEffectDesc(6);

						gameState.SylvaniSpeaks = true;
					}
				}

				// You hear sounds...

				if (!gameState.ScuffleSoundsHeard && characterRoom.Uid == 26)
				{
					Globals.Engine.PrintEffectDesc(15);

					gameState.ScuffleSoundsHeard = true;
				}
			}

			base.ProcessEvents();
		}
	}
}
