
// Delegates.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;

namespace Eamon.Framework
{
	/// <summary>
	/// A collection of C# delegate signatures.
	/// </summary>
	public static class Delegates
	{
		/// <summary>
		/// Queries the game database for a list of <see cref="IArtifact">Artifact</see>s matching a criteria set.
		/// </summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		public delegate IList<IArtifact> GetArtifactListFunc(params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary>
		/// Queries the game database for a list of <see cref="IMonster">Monster</see>s matching a criteria set.
		/// </summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		public delegate IList<IMonster> GetMonsterListFunc(params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary>
		/// Filters a given <see cref="IArtifact">Artifact</see> list, returning all records matching a given name.
		/// </summary>
		/// <param name="artifactList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IArtifact> FilterArtifactListFunc(IList<IArtifact> artifactList, string name);

		/// <summary>
		/// Filters a given <see cref="IMonster">Monster</see> list, returning all records matching a given name.
		/// </summary>
		/// <param name="monsterList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IMonster> FilterMonsterListFunc(IList<IMonster> monsterList, string name);

		/// <summary>
		/// Filters a given record list, returning all records matching a given name.
		/// </summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IGameBase> FilterRecordListFunc(IList<IGameBase> recordList, string name);

		/// <summary>
		/// Reveals an embedded <see cref="IArtifact">Artifact</see>, moving it into its containing <see cref="IRoom">Room</see>
		/// and printing its <see cref="IGameBase.Desc">Desc</see>ription if necessary.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		public delegate void RevealEmbeddedArtifactFunc(IRoom room, IArtifact artifact);
	}
}
