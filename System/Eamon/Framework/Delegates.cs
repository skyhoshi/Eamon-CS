
// Delegates.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework
{
	/// <summary></summary>
	public static class Delegates
	{
		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		public delegate IList<IArtifact> GetArtifactListFunc(params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		public delegate IList<IMonster> GetMonsterListFunc(params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IArtifact> FilterArtifactListFunc(IList<IArtifact> artifactList, string name);

		/// <summary></summary>
		/// <param name="monsterList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IMonster> FilterMonsterListFunc(IList<IMonster> monsterList, string name);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IGameBase> FilterRecordListFunc(IList<IGameBase> recordList, string name);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		public delegate void RevealEmbeddedArtifactFunc(IRoom room, IArtifact artifact);
	}
}
