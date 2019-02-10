
// IRecordDb.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.DataStorage.Generic
{
	/// <summary></summary>
	public interface IRecordDb<T>
	{
		/// <summary></summary>
		bool CopyAddedRecord { get; set; }

		/// <summary></summary>
		T this[long uid] { get; set; }
	}
}
