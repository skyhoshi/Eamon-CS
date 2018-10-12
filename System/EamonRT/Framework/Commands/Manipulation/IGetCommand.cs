
// IGetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	public interface IGetCommand : ICommand
	{
		bool GetAll { get; set; }
	}
}
