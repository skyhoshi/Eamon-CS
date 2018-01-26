
// IRecordDb.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.DataStorage.Generic
{
	public interface IRecordDb<T>
	{
		bool CopyAddedRecord { get; set; }

		T this[long uid] { get; set; }
	}
}
