
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="effectUid"></param>
		/// <returns></returns>
		string GetMonsterCurse(IMonster monster, long effectUid);
	}
}
