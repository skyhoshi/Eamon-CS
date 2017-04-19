
// IDdEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace EamonDD.Framework
{
	public interface IDdEngine
	{
		bool IsAdventureFilesetLoaded();

		void ProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag);
	};
}
