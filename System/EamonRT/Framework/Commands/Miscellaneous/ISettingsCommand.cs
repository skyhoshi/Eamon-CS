
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	public interface ISettingsCommand : ICommand
	{
		bool? VerboseRooms { get; set; }

		bool? VerboseMonsters { get; set; }

		bool? VerboseArtifacts { get; set; }

		long? PauseCombatMs { get; set; }
	}
}
