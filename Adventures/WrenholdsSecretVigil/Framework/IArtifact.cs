
// IArtifact.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace WrenholdsSecretVigil.Framework
{
	public interface IArtifact : Eamon.Framework.IArtifact
	{
		bool IsBuriedInRoomUid(long roomUid);

		bool IsBuriedInRoom(Eamon.Framework.IRoom room);
	}
}
