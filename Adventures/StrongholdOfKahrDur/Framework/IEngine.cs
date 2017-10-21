
// IEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace StrongholdOfKahrDur.Framework
{
	public interface IEngine : EamonRT.Framework.IEngine
	{
		bool SpellReagentsInCauldron(Eamon.Framework.IArtifact artifact);
	}
}
