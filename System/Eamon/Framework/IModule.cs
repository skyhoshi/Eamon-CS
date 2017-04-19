
// IModule.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.DataEntry;
using Eamon.Framework.Validation;

namespace Eamon.Framework
{
	public interface IModule : IHaveUid, IHaveFields, IValidator, IEditable, IComparable<IModule>
	{
		#region Properties

		string Name { get; set; }

		string Desc { get; set; }

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
