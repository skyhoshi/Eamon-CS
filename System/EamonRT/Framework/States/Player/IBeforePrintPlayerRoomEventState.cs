
// IBeforePrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IBeforePrintPlayerRoomEventState : IState
	{
		/// <summary></summary>
		bool ExitEventLoop { get; set; }
	}
}
