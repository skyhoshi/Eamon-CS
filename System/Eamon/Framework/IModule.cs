
// IModule.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;

namespace Eamon.Framework
{
	public interface IModule : IGameBase, IComparable<IModule>
	{
		#region Properties

		string Author { get; set; }

		string VolLabel { get; set; }

		string SerialNum { get; set; }

		DateTime LastMod { get; set; }

		long IntroStory { get; set; }

		long NumDirs { get; set; }

		long NumRooms { get; set; }

		long NumArtifacts { get; set; }

		long NumEffects { get; set; }

		long NumMonsters { get; set; }

		long NumHints { get; set; }

		#endregion

		#region Methods

		void PrintInfo();

		#endregion
	}
}
