
// IRoom.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Enums = Eamon.Framework.Primitive.Enums;

namespace WrenholdsSecretVigil.Framework
{
	public interface IRoom : Eamon.Framework.IRoom
	{
		bool IsDigCommandAllowedInRoom();

		bool IsDirectionEffect(long index);

		bool IsDirectionEffect(Enums.Direction dir);

		long GetDirectionEffectUid(Enums.Direction dir);

		Eamon.Framework.IEffect GetDirectionEffect(Enums.Direction dir);
	}
}
