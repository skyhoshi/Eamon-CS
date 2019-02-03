
// Delegates.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework
{
	public static class Delegates
	{
		public delegate IList<IArtifact> GetArtifactListFunc(params Func<IArtifact, bool>[] whereClauseFuncs);

		public delegate IList<IMonster> GetMonsterListFunc(params Func<IMonster, bool>[] whereClauseFuncs);

		public delegate IList<IArtifact> FilterArtifactListFunc(IList<IArtifact> artifactList, string name);

		public delegate IList<IMonster> FilterMonsterListFunc(IList<IMonster> monsterList, string name);

		public delegate IList<IGameBase> FilterRecordListFunc(IList<IGameBase> recordList, string name);

		public delegate void RevealEmbeddedArtifactFunc(IRoom room, IArtifact artifact);
	}
}
