
// IArmor.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IArmor
	{
		string Name { get; set; }

		string MarcosName { get; set; }

		long MarcosPrice { get; set; }

		long MarcosNum { get; set; }

		long ArtifactValue { get; set; }
	}
}
