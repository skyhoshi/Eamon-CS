
// IAfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IAfterPlayerMoveState : IState
	{
		/// <summary></summary>
		Enums.Direction Direction { get; set; }

		/// <summary></summary>
		IArtifact Artifact { get; set; }

		/// <summary></summary>
		bool MoveMonsters { get; set; }
	}
}
