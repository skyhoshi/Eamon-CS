
// IEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace WrenholdsSecretVigil.Framework
{
	public interface IEngine : EamonRT.Framework.IEngine
	{
		string GetMonsterCurse(Eamon.Framework.IMonster monster, long effectUid);
	}
}
