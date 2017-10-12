
// IFileset.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;

namespace Eamon.Framework
{
	public interface IFileset : IGameBase, IComparable<IFileset>
	{
		#region Properties

		string WorkDir { get; set; }

		string PluginFileName { get; set; }

		string ConfigFileName { get; set; }

		string FilesetFileName { get; set; }

		string CharacterFileName { get; set; }

		string ModuleFileName { get; set; }

		string RoomFileName { get; set; }

		string ArtifactFileName { get; set; }

		string EffectFileName { get; set; }

		string MonsterFileName { get; set; }

		string HintFileName { get; set; }

		string GameStateFileName { get; set; }

		#endregion

		#region Methods

		RetCode DeleteFiles(string fieldName, bool useFilePrefix);

		#endregion
	}
}
