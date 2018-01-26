
// IPlayerDeadState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.States;

namespace EamonRT.Framework.States
{
	public interface IPlayerDeadState : IState
	{
		bool PrintLineSep { get; set; }
	}
}
