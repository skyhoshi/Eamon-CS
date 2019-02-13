
// IProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IProcessPlayerInputState : IState
	{
		/// <summary></summary>
		bool IncrementCurrTurn { get; set; }
	}
}
