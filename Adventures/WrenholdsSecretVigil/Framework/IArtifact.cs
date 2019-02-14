
// IArtifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IArtifact : Eamon.Framework.IArtifact
	{
		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsBuriedInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <returns></returns>
		bool IsBuriedInRoom(Eamon.Framework.IRoom room);
	}
}
