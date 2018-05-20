
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace StrongholdOfKahrDur.Framework
{
	public interface IEngine : EamonRT.Framework.IEngine
	{
		bool SpellReagentsInCauldron(IArtifact cauldronArtifact);
	}
}
