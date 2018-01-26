
// IDbTable.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework.DataStorage.Generic
{
	public interface IDbTable<T> where T : class, IGameBase
	{
		ICollection<T> Records { get; set; }

		IList<long> FreeUids { get; set; }

		T[] Cache { get; set; }

		long CurrUid { get; set; }

		RetCode FreeRecords(bool dispose = true);

		long GetRecordsCount();

		T FindRecord(long uid);

		T FindRecord(Type type, bool exactMatch = false);

		RetCode AddRecord(T record, bool makeCopy = false);

		T RemoveRecord(long uid);

		T RemoveRecord(Type type, bool exactMatch = false);

		long GetRecordUid(bool allocate = true);

		void FreeRecordUid(long uid);
	}
}
