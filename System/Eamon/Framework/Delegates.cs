
// Delegates.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework
{
	/// <summary>
	/// A collection of C# delegate signatures.
	/// </summary>
	/// <remarks>
	/// These delegates are used by <see cref="Parsing.IParserData"/> to support player command parsing, 
	/// direct object and indirect object resolution.  There are methods that match these signatures in
	/// <see cref="IEngine"/> and <see cref="EamonRT.Framework.IEngine"/> that the parser uses as default
	/// implementations; these are detailed below.
	/// </remarks>
	public static class Delegates
	{
		/// <summary>
		/// Queries the game database for a list of artifacts matching a criteria set.
		/// </summary>
		/// <param name="shouldQueryFunc">
		/// The should query function; this is evaluated before proceeding with the main query. If it returns true,
		/// the main query proceeds; if false, the main query never happens and an empty list is returned.  Typically
		/// this will be a simple lambda like () => true, but it can vary based on the given circumstances (eg, maybe
		/// the room is dark).
		/// </param>
		/// <param name="whereClauseFuncs">
		/// The set of methods used to match artifacts in the game database.  Each method is used as a query Where
		/// clause and the resulting artifacts are added to the list.  This means the database is queried once for
		/// each member of the set.  Note that depending on the methods it is possible for the same artifact to
		/// show up in the list more than once.
		/// </param>
		/// <returns>A list of artifacts matching the given criteria set.  At minimum an empty list should be returned (never null).</returns>
		/// <seealso cref="IEngine.GetArtifactList"/>
		public delegate IList<IArtifact> GetArtifactListFunc(Func<bool> shouldQueryFunc, params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary>
		/// Queries the game database for a list of monsters matching a criteria set.
		/// </summary>
		/// <param name="shouldQueryFunc">
		/// The should query function; this is evaluated before proceeding with the main query. If it returns true,
		/// the main query proceeds; if false, the main query never happens and an empty list is returned.  Typically
		/// this will be a simple lambda like () => true, but it can vary based on the given circumstances (eg, maybe
		/// the room is dark).
		/// </param>
		/// <param name="whereClauseFuncs">
		/// The set of methods used to match monsters in the game database.  Each method is used as a query Where
		/// clause and the resulting monsters are added to the list.  This means the database is queried once for
		/// each member of the set.  Note that depending on the methods it is possible for the same monster to
		/// show up in the list more than once.
		/// </param>
		/// <returns>A list of monsters matching the given criteria set.  At minimum an empty list should be returned (never null).</returns>
		/// <seealso cref="IEngine.GetMonsterList"/>
		public delegate IList<IMonster> GetMonsterListFunc(Func<bool> shouldQueryFunc, params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary>
		/// Filters a given artifact list, returning all records matching a given name.
		/// </summary>
		/// <param name="artifactList">The list of artifacts to filter.</param>
		/// <param name="name">The name to search for.</param>
		/// <remarks>
		/// The passed in artifact list may be searched using a variety of techniques, including direct artifact name
		/// match, plural name match and synonym match.  These techniques may be augmented with partial matching (that
		/// is, start or end of string) if all else fails.  The passed in artifact list will not be altered and the
		/// returned list can contain multiple artifacts.
		/// </remarks>
		/// <returns>A list of artifacts matching the given name.  At minimum an empty list should be returned (never null).</returns>
		/// <seealso cref="EamonRT.Framework.IEngine.FilterArtifactList"/>
		public delegate IList<IArtifact> FilterArtifactListFunc(IList<IArtifact> artifactList, string name);

		/// <summary>
		/// Filters a given monster list, returning all records matching a given name.
		/// </summary>
		/// <param name="monsterList">The list of monsters to filter.</param>
		/// <param name="name">The name to search for.</param>
		/// <remarks>
		/// The passed in monster list may be searched using a variety of techniques, including direct monster name
		/// match, plural name match and synonym match.  These techniques may be augmented with partial matching (that
		/// is, start or end of string) if all else fails.  The passed in monster list will not be altered and the
		/// returned list can contain multiple monsters.
		/// </remarks>
		/// <returns>A list of monsters matching the given name.  At minimum an empty list should be returned (never null).</returns>
		/// <seealso cref="EamonRT.Framework.IEngine.FilterMonsterList"/>
		public delegate IList<IMonster> FilterMonsterListFunc(IList<IMonster> monsterList, string name);

		/// <summary>
		/// Filters a given record list, returning all records matching a given name.
		/// </summary>
		/// <param name="recordList">The list of records to filter.  These records can be artifacts, monsters or both.</param>
		/// <param name="name">The name to search for.</param>
		/// <remarks>
		/// The passed in record list may be searched using a variety of techniques, including direct record name match,
		/// plural name match and synonym match.  These techniques may be augmented with partial matching (that is, start
		/// or end of string) if all else fails.  The passed in record list will not be altered and the returned list can
		/// contain multiple records.
		/// </remarks>
		/// <returns>A list of records matching the given name.  At minimum an empty list should be returned (never null).</returns>
		/// <seealso cref="EamonRT.Framework.IEngine.FilterRecordList"/>
		public delegate IList<IGameBase> FilterRecordListFunc(IList<IGameBase> recordList, string name);

		/// <summary>
		/// Reveals an embedded artifact, moving it into its containing room and printing its description if necessary.
		/// </summary>
		/// <param name="room">The room containing the embedded artifact.</param>
		/// <param name="artifact">The embedded artifact to reveal.</param>
		/// <remarks>
		/// If the artifact is embedded within the room, it is revealed; moved into the room's regular inventory list, and
		/// its description is printed if it hasn't been seen.  While this task is typically performed during resolution of
		/// the direct or indirect object during player command parsing, you can reveal embedded artifacts at any appropriate
		/// point during the game by calling the method explicitly.
		/// </remarks>
		/// <seealso cref="EamonRT.Framework.IEngine.RevealEmbeddedArtifact"/>
		public delegate void RevealEmbeddedArtifactFunc(IRoom room, IArtifact artifact);
	}
}
