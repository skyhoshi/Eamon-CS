
// IRoom.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		/// <returns></returns>
		bool IsDigCommandAllowedInRoom();

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionEffect(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionEffect(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		long GetDirectionEffectUid(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		IEffect GetDirectionEffect(Enums.Direction dir);
	}
}
