
// IHaveUid.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;

namespace Eamon.Framework
{
	public interface IHaveUid : IDisposable
	{
		long Uid { get; set; }

		bool IsUidRecycled { get; set; }
	}
}
