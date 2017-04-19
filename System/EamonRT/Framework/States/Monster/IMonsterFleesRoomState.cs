
// IMonsterFleesRoomState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.States;

namespace EamonRT.Framework.States
{
	public interface IMonsterFleesRoomState : IState
	{
		bool FleeCommandCalled { get; set; }

		long GroupCount { get; set; }
	}
}
