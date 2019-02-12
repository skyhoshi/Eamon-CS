
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonDD.Framework
{
	/// <summary></summary>
	public interface IEngine : Eamon.Framework.IEngine
	{
		/// <summary></summary>
		bool IsAdventureFilesetLoaded();

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="ddfnFlag"></param>
		/// <param name="nlFlag"></param>
		void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag);
	};
}
