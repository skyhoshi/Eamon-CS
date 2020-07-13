
// IAfterPrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IAfterPrintPlayerRoomEventState : IState
	{
		/// <summary></summary>
		bool ExitEventLoop { get; set; }
	}
}
