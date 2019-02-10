
// IArtifactLinkage.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IArtifactLinkage
	{
		/// <summary></summary>
		long RoomUid { get; set; }

		/// <summary></summary>
		long ArtifactUid1 { get; set; }

		/// <summary></summary>
		long ArtifactUid2 { get; set; }
	}
}
