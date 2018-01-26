
// IFleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.Commands
{
	public interface IFleeCommand : ICommand
	{
		Enums.Direction Direction { get; set; }
	}
}
