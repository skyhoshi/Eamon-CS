
// IStateImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.States
{
	public interface IStateImpl : IStateSignatures
	{
		IState State { get; set; }
	}
}
