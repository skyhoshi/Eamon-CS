
// IWordWrapArgs.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Args
{
	/// <summary></summary>
	public interface IWordWrapArgs
	{
		/// <summary></summary>
		long CurrColumn { get; set; }

		/// <summary></summary>
		char LastChar { get; set; }
	}
}
