
// IEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace EamonDD.Framework
{
	public interface IEngine : Eamon.Framework.IEngine
	{
		bool IsAdventureFilesetLoaded();

		void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag);
	};
}
