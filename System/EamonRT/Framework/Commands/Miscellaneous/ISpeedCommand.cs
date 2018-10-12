
// ISpeedCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	public interface ISpeedCommand : ICommand
	{
		bool CastSpell { get; set; }
	}
}
