
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace WrenholdsSecretVigil.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		long MedallionCharges { get; set; }

		long SlimeBlasts { get; set; }

		bool PulledRope { get; set; }

		bool RemovedLifeOrb { get; set; }

		bool[] MonsterCurses { get; set; }

		#endregion

		#region Methods

		bool GetMonsterCurses(long index);

		void SetMonsterCurses(long index, bool value);

		#endregion
	}
}
