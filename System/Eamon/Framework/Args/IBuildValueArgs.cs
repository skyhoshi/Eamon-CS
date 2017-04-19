
// IBuildValueArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;

namespace Eamon.Framework.Args
{
	public interface IBuildValueArgs
	{
		long BufSize { get; set; }

		char FillChar { get; set; }

		long Offset { get; set; }

		StringBuilder Buf { get; set; }
	}
}
