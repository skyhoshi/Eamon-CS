
// IPrintDescArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Args
{
	public interface IPrintDescArgs
	{
		bool EditRec { get; set; }

		bool EditField { get; set; }

		Enums.FieldDesc FieldDesc { get; set; }

		StringBuilder Buf { get; set; }
	}
}
