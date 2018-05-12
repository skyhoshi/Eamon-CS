
// IRoom.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace WrenholdsSecretVigil.Framework
{
	public interface IRoom : Eamon.Framework.IRoom
	{
		bool IsDigCommandAllowedInRoom();

		bool IsDirectionEffect(long index);

		bool IsDirectionEffect(Enums.Direction dir);

		long GetDirectionEffectUid(Enums.Direction dir);

		IEffect GetDirectionEffect(Enums.Direction dir);
	}
}
