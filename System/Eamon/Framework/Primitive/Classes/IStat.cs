
// IStat.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IStat
	{
		string Name { get; set; }

		string Abbr { get; set; }

		string EmptyVal { get; set; }

		long MinValue { get; set; }

		long MaxValue { get; set; }
	}
}
