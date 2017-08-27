
// PoundCharPolicy.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	public enum PoundCharPolicy : long
	{
		None = 0,					// no pound chars on artifact names
		PlayerArtifactsOnly,		// pound chars only on player artifact names
		AllArtifacts				// pound chars on all artifacts in database
	}
}
