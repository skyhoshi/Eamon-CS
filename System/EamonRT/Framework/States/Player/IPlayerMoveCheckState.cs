
// IPlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	public interface IPlayerMoveCheckState : IState
	{
		Enums.Direction Direction { get; set; }

		IArtifact Artifact { get; set; }

		bool Fleeing { get; set; }
	}
}
