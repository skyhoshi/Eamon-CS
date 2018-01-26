
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace TheTrainingGround.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		bool RedSunSpeaks { get; set; }

		bool JacquesShouts { get; set; }

		bool JacquesRecoversRapier { get; set; }

		bool KoboldsAppear { get; set; }

		bool SylvaniSpeaks { get; set; }

		bool ThorsHammerAppears { get; set; }

		bool LibrarySecretPassageFound { get; set; }

		bool ScuffleSoundsHeard { get; set; }

		bool CharismaBoosted { get; set; }

		long GenderChangeCounter { get; set; }

		#endregion
	}
}
