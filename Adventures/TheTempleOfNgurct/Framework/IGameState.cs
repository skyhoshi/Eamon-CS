
// IGameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace TheTempleOfNgurct.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		long WanderingMonster { get; set; }

		long DwLoopCounter { get; set; }

		long WandCharges { get; set; }

		long Regenerate { get; set; }

		long KeyRingRoomUid { get; set; }

		bool AlkandaKilled { get; set; }

		bool AlignmentConflict { get; set; }

		bool CobraAppeared { get; set; }

		#endregion
	}
}
