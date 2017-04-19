
// IMonsterReadiesWeaponState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.States;

namespace EamonRT.Framework.States
{
	public interface IMonsterReadiesWeaponState : IState
	{
		IList<IArtifact> ArtifactList { get; set; }

		long ArtifactListIndex { get; set; }

		long MemberNumber { get; set; }

		bool GetCommandCalled { get; set; }

		bool ReadyCommandCalled { get; set; }
	}
}
