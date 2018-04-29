
// IDbTable.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework.DataStorage.Generic
{
	/// <summary>
	/// Represents a collection of records of type T in a database.
	/// </summary>
	/// <typeparam name="T">The interface type of the records.</typeparam>
	/// <remarks>
	/// This defines the concept of a database table in Eamon CS.  It is integral to the storage of all
	/// game-related records (rooms, artifacts, monsters, etc), both in memory and in filesystem.  The
	/// <see cref="IDatabase"/> interface exposes a set of IDbTables, one for each record type derived
	/// from <see cref="IGameBase"/>.  The database table class heirarchy is derived from the abstract
	/// class <see cref="Game.DataStorage.Generic.DbTable{T}"/>.  If you look inside the various game
	/// .XML files you will see that they consist of the serialized concretes of IDbTable mentioned
	/// below in the See Also section.
	/// </remarks>
	/// <seealso cref = "Game.DataStorage.ConfigDbTable" />
	/// <seealso cref = "Game.DataStorage.FilesetDbTable" />
	/// <seealso cref = "Game.DataStorage.CharacterDbTable" />
	/// <seealso cref = "Game.DataStorage.ModuleDbTable" />
	/// <seealso cref = "Game.DataStorage.RoomDbTable" />
	/// <seealso cref = "Game.DataStorage.ArtifactDbTable" />
	/// <seealso cref = "Game.DataStorage.EffectDbTable" />
	/// <seealso cref = "Game.DataStorage.MonsterDbTable" />
	/// <seealso cref = "Game.DataStorage.HintDbTable" />
	/// <seealso cref = "Game.DataStorage.GameStateDbTable" />
	public interface IDbTable<T> where T : class, IGameBase
	{
		/// <summary>
		/// This is the collection of records stored in the database table.
		/// </summary>
		/// <remarks>
		/// The records stored here are keyed using their Uid property, which as the name implies is
		/// expected to be unique.  Records can be added, removed or modified dynamically at runtime
		/// as needed.  If you look in the various .XML game files you will see that you can actually
		/// store records of type T, or any derivative of T.  This is why, for example, if you look
		/// in MONSTERS.XML for "Test Adventure" and "A Runcible Cargo" you will see records of type
		/// <see cref="Game.Monster"/> and <see cref="ARuncibleCargo.Game.Monster"/>, respectively.
		/// This polymorphic behavior is at the heart of custom adventure development in Eamon CS.
		/// </remarks>
		ICollection<T> Records { get; set; }

		/// <summary>
		/// A collection of Uids available for reuse by new instances of this record type; may be empty.
		/// </summary>
		/// <remarks>
		/// This property is intended to be used in conjunction with <see cref="GetRecordUid(bool)"/> and
		/// <see cref="FreeRecordUid(long)"/>.  As a general rule you should rely on these methods to 
		/// manipulate the FreeUids list.
		/// </remarks>
		IList<long> FreeUids { get; set; }

		/// <summary>
		/// A quick-lookup cache of records stored in most recently used (MRU) order.
		/// </summary>
		/// <remarks>
		/// This cache contains the records of type T most recently used by the system.  The <see cref="Records"/>
		/// collection is currently implemented as a B-Plus Tree, but lookups amount to full table scans so the cache
		/// is always checked first.  The size of the cache is set by <see cref="IEngine.NumCacheItems"/> and old
		/// records will fall out as new records are added.
		/// </remarks>
		T[] Cache { get; set; }

		/// <summary>
		/// A sequence number used to store the last unique ID allocated to a record of type T.
		/// </summary>
		/// <remarks>
		/// Each record type gets its own sequence number to draw Uids from.  Uids start at one (1), and zero (0)
		/// is considered null or invalid.  The <see cref="GetRecordUid"/> and <see cref="FreeRecordUid(long)"/> 
		/// methods should be used to manage all Uid allocations and deallocations, respectively.  When a Uid is
		/// freed, either CurrUid is decremented (when Uid equals CurrUid), or the Uid is added to
		/// <see cref="FreeUids"/> for later reuse.  
		/// </remarks>
		long CurrUid { get; set; }

		/// <summary>
		/// Fully reinitializes the IDbTable and restores it to its initial (empty) state.
		/// </summary>
		/// <param name="dispose">Optionally call Dispose method on every freed record.</param>
		/// <remarks>
		/// This method clears out anything stored in <see cref="Records"/>, optionally calling Dispose on every record
		/// freed.  It also clears the <see cref="Cache"/> and <see cref="FreeUids"/>, and resets <see cref="CurrUid"/>
		/// to zero (0).  The empty IDbTable is ready to be reused.
		/// </remarks>
		/// <returns>Success</returns>
		RetCode FreeRecords(bool dispose = true);

		/// <summary>
		/// Gets the number of records of type T stored in the Records collection.
		/// </summary>
		long GetRecordsCount();

		/// <summary>
		/// Gets a record from the Records collection.
		/// </summary>
		/// <param name="uid">The Uid of the record to find.</param>
		/// <remarks>
		/// This method finds and returns a record of type T with the given Uid.  If the record is not found, the
		/// method returns null.  The search begins in the <see cref="Cache"/> and on failure, <see cref="Records"/>
		/// is searched.  Any record found is either moved or added to the head of the Cache; on overflow, the least
		/// recently used (LRU) record will be ejected.
		/// </remarks>
		/// <returns>The record with the given Uid or null if not found.</returns>
		T FindRecord(long uid);

		/// <summary>
		/// Gets a record from the Records collection based on interface type.
		/// </summary>
		/// <param name="type">The interface type to find.</param>
		/// <param name="exactMatch">Optionally try to find a record directly implementing the interface.</param>
		/// <remarks>
		/// This method finds and returns the first record of type T that matches the provided interface type.  If
		/// the record directly implements the interface, it will always match.  If the interface is an ancestor then
		/// exactMatch must be false for a match to occur.
		/// or an ancestor.
		/// </remarks>
		/// <returns>The first record matching the provided interface type or null if not found.</returns>
		T FindRecord(Type type, bool exactMatch = false);

		/// <summary>
		/// Adds a record to the Records collection.
		/// </summary>
		/// <param name="record">The record to add.</param>
		/// <param name="makeCopy">Optionally clone the record and add the clone instead.</param>
		/// <remarks>
		/// This method adds a record of type T to <see cref="Records"/>.  It is assumed the record has been properly
		/// initialized and its Uid has been assigned.  If a record with the same Uid already exists in the collection
		/// the attempt to add the new one will fail.  You can optionally choose to do a deep clone of the record and
		/// add the clone instead if you would prefer.
		/// </remarks>
		/// <returns>Success, InvalidArg, AlreadyExists, OutOfMemory</returns>
		RetCode AddRecord(T record, bool makeCopy = false);

		/// <summary>
		/// Removes a record from the Records collection.
		/// </summary>
		/// <param name="uid">The Uid of the record to remove.</param>
		/// <remarks>
		/// This method removes a record of type T with the given Uid from <see cref="Records"/> and (if necessary)
		/// <see cref="Cache"/> and returns it.  If the record is not found, the method returns null.  If the record
		/// is no longer needed and its IsUidRecycled property is true, you should call Dispose on the record to
		/// ensure the Uid is freed; otherwise you must do this manually using <see cref="FreeRecordUid(long)"/>.
		/// </remarks>
		/// <returns>The record with the given Uid or null if not found.</returns>
		T RemoveRecord(long uid);

		/// <summary>
		/// Removes a record from the Records collection based on interface type.
		/// </summary>
		/// <param name="type">The interface type to remove.</param>
		/// <param name="exactMatch">Optionally try to find a record directly implementing the interface.</param>
		/// <remarks>
		/// This method removes the first record of type T that matches the provided interface type from both
		/// <see cref="Records"/> and (if necessary) <see cref="Cache"/>.  The record, if found, is returned;
		/// otherwise null is returned.  Take a look at the comments for <see cref="FindRecord(Type, bool)"/>
		/// and <see cref="RemoveRecord(long)"/> because they also apply to this method.
		/// <returns>The first record matching the provided interface type or null if not found.</returns>
		T RemoveRecord(Type type, bool exactMatch = false);

		/// <summary>
		/// Gets the next available record Uid.
		/// </summary>
		/// <param name="allocate">Optionally allocate the Uid for use (this is the default).</param>
		/// <remarks>
		/// This method manages the allocation of Uids for records of type T.  If the allocate parameter is
		/// true, then a Uid will be allocated; otherwise, it simply returns the value of <see cref="CurrUid"/>,
		/// the most recently allocated Uid.  During allocation, if the <see cref="FreeUids"/> list is not empty,
		/// the head of the list is removed and returned.  Otherwise, the CurrUid is incremented and returned.
		/// Records that have Uids allocated by this method should always set IsUidRecycled to true.  Then either
		/// the record's Dispose method or <see cref="FreeRecordUid(long)"/> can be called (but not both) to free
		/// the Uid so it can later be reused.
		/// </remarks>
		/// <returns>Either the next allocated Uid or most recently allocated Uid; else returns -1 on failure.</returns>
		long GetRecordUid(bool allocate = true);

		/// <summary>
		/// Frees a record Uid, making it available again for use.
		/// </summary>
		/// <param name="uid">The record Uid to free.</param>
		/// <remarks>
		/// This method is used to free a Uid allocated to a record of type T, making it available again for use.  
		/// You should always free any Uid allocated with <see cref="GetRecordUid(bool)"/>, either indirectly using
		/// the record's Dispose method or directly using FreeRecordUid (but never both).  When the Uid is equal to
		/// <see cref="CurrUid"/>, the latter is simply decremented; otherwise it is added to the tail of the 
		/// <see cref="FreeUids"/> list.  Due to the minimal validation done on the passed in Uid, it is the
		/// caller's responsibility to ensure it is valid.
		/// </remarks>
		void FreeRecordUid(long uid);
	}
}
