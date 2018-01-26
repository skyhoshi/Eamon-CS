
// IArtifactLinkage.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IArtifactLinkage
	{
		long RoomUid { get; set; }

		long ArtifactUid1 { get; set; }

		long ArtifactUid2 { get; set; }
	}
}
