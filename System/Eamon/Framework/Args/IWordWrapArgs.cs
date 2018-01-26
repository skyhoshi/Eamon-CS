
// IWordWrapArgs.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Args
{
	public interface IWordWrapArgs
	{
		long CurrColumn { get; set; }

		char LastChar { get; set; }
	}
}
