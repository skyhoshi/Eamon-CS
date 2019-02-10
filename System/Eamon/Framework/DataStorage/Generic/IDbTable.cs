
// IDbTable.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework.DataStorage.Generic
{
	/// <summary></summary>
	public interface IDbTable<T> where T : class, IGameBase
	{
		/// <summary></summary>
		ICollection<T> Records { get; set; }

		/// <summary></summary>
		IList<long> FreeUids { get; set; }

		/// <summary></summary>
		T[] Cache { get; set; }

		/// <summary></summary>
		long CurrUid { get; set; }

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeRecords(bool dispose = true);

		/// <summary></summary>
		/// <returns></returns>
		long GetRecordsCount();

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		T FindRecord(long uid);

		/// <summary></summary>
		/// <param name="type"></param>
		/// <param name="exactMatch"></param>
		/// <returns></returns>
		T FindRecord(Type type, bool exactMatch = false);

		/// <summary></summary>
		/// <param name="record"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddRecord(T record, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		T RemoveRecord(long uid);

		/// <summary></summary>
		/// <param name="type"></param>
		/// <param name="exactMatch"></param>
		/// <returns></returns>
		T RemoveRecord(Type type, bool exactMatch = false);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetRecordUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeRecordUid(long uid);
	}
}
