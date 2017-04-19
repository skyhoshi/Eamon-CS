
// IDirection.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IDirection
	{
		string Name { get; set; }

		string PrintedName { get; set; }

		string Abbr { get; set; }
	}
}
