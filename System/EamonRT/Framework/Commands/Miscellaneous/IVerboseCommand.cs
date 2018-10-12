
// IVerboseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	public interface IVerboseCommand : ICommand
	{
		bool VerboseRooms { get; set; }

		bool VerboseArtifacts { get; set; }

		bool VerboseMonsters { get; set; }
	}
}
