
// IErrorState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.States
{
	public interface IErrorState : IState
	{
		long ErrorCode { get; set; }

		string ErrorMessage { get; set; }
	}
}
