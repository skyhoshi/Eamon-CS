
// IListArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;

namespace Eamon.Framework.Args
{
	public interface IListArgs
	{
		bool FullDetail { get; set; }

		bool ShowDesc { get; set; }

		bool ResolveEffects { get; set; }

		bool LookupMsg { get; set; }

		bool NumberFields { get; set; }

		bool ExcludeROFields { get; set; }

		StringBuilder Buf { get; set; }

		StringBuilder Buf01 { get; set; }

		long ListNum { get; set; }
	}
}
