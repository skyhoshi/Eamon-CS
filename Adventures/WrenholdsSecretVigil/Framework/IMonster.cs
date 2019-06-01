
// IMonster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IMonster : Eamon.Framework.IMonster
	{
		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool ShouldRefuseToAcceptGift01(Eamon.Framework.IArtifact artifact);
	}
}
