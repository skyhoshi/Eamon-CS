
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace WrenholdsSecretVigil.Framework
{
	public interface IEngine : EamonRT.Framework.IEngine
	{
		string GetMonsterCurse(IMonster monster, long effectUid);
	}
}
