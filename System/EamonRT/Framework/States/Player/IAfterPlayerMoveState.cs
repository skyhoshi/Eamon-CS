
// IAfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IAfterPlayerMoveState : IState
	{
		/// <summary></summary>
		Direction Direction { get; set; }

		/// <summary></summary>
		IArtifact Artifact { get; set; }

		/// <summary></summary>
		bool MoveMonsters { get; set; }
	}
}
