
// ISaveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Commands;

namespace EamonRT.Framework.Commands
{
	public interface ISaveCommand : ICommand
	{
		long SaveSlot { get; set; }

		string SaveName { get; set; }
	}
}
