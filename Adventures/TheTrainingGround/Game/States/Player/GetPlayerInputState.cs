
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeCommandPromptPrint && ShouldPreTurnProcess())
			{
				var characterMonster = gMDB[gGameState.Cm];

				Debug.Assert(characterMonster != null);

				var characterRoom = characterMonster.GetInRoom();

				Debug.Assert(characterRoom != null);

				// Events only occur in lit rooms

				if (characterRoom.IsLit())
				{
					var redSunMonster = gMDB[1];

					Debug.Assert(redSunMonster != null);

					// Red Sun speaks

					if (!gGameState.RedSunSpeaks && redSunMonster.IsInRoom(characterRoom))
					{
						gEngine.PrintEffectDesc(4);

						gGameState.RedSunSpeaks = true;
					}

					// Jacques shouts from behind door

					if (!gGameState.JacquesShouts && characterRoom.Uid == 8)
					{
						gEngine.PrintEffectDesc(5);

						gGameState.JacquesShouts = true;
					}

					var sylvaniMonster = gMDB[12];

					Debug.Assert(sylvaniMonster != null);

					// Sylvani speaks

					if (!gGameState.SylvaniSpeaks && sylvaniMonster.IsInRoom(characterRoom))
					{
						gEngine.PrintEffectDesc(6);

						gGameState.SylvaniSpeaks = true;
					}
				}

				// You hear sounds...

				if (!gGameState.ScuffleSoundsHeard && characterRoom.Uid == 26)
				{
					gEngine.PrintEffectDesc(15);

					gGameState.ScuffleSoundsHeard = true;
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
