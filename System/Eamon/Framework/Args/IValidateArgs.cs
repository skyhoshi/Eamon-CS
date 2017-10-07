
// IValidateArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Text;

namespace Eamon.Framework.Args
{
	public interface IValidateArgs
	{
		IField ErrorField { get; set; }

		string ErrorMessage { get; set; }

		StringBuilder Buf { get; set; }

		Type RecordType { get; set; }

		IGameBase EditRecord { get; set; }

		long NewRecordUid { get; set; }

		bool ShowDesc { get; set; }

      void Clear();
	}
}
