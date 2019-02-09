
// IModule.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IModule : IGameBase, IComparable<IModule>
	{
		#region Properties

		/// <summary></summary>
		string Author { get; set; }

		/// <summary></summary>
		string VolLabel { get; set; }

		/// <summary></summary>
		string SerialNum { get; set; }

		/// <summary></summary>
		DateTime LastMod { get; set; }

		/// <summary></summary>
		long IntroStory { get; set; }

		/// <summary></summary>
		long NumDirs { get; set; }

		/// <summary></summary>
		long NumRooms { get; set; }

		/// <summary></summary>
		long NumArtifacts { get; set; }

		/// <summary></summary>
		long NumEffects { get; set; }

		/// <summary></summary>
		long NumMonsters { get; set; }

		/// <summary></summary>
		long NumHints { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		void PrintInfo();

		#endregion
	}
}
