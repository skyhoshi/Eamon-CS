
// ISpell.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface ISpell
	{
		string Name { get; set; }

		string HokasName { get; set; }

		long HokasPrice { get; set; }

		long MinValue { get; set; }

		long MaxValue { get; set; }
	}
}
