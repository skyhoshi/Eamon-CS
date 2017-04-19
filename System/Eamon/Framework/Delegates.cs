
// Delegates.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.DataStorage;

namespace Eamon.Framework
{
	public static class Delegates
	{
		public delegate RetCode GetDatabaseFunc(long dbIndex, ref IDatabase database);

		public delegate RetCode GetDbIndexFunc(ref long dbIndex);

		public delegate RetCode GetDbIndexStackSizeFunc(ref long size);

		public delegate IList<IArtifact> GetArtifactListFunc(Func<bool> shouldQueryFunc, params Func<IArtifact, bool>[] whereClauseFuncs);

		public delegate IList<IMonster> GetMonsterListFunc(Func<bool> shouldQueryFunc, params Func<IMonster, bool>[] whereClauseFuncs);

		public delegate IList<IArtifact> FilterArtifactListFunc(IList<IArtifact> artifactList, string name);

		public delegate IList<IMonster> FilterMonsterListFunc(IList<IMonster> monsterList, string name);

		public delegate IList<IHaveListedName> FilterRecordListFunc(IList<IHaveListedName> recordList, string name);

		public delegate void RevealEmbeddedArtifactFunc(IRoom room, IArtifact artifact);
	}
}
