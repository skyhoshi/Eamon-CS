
// Thread.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class Thread : IThread
	{
		public virtual void Sleep(long milliseconds)
		{
			System.Threading.Thread.Sleep((int)milliseconds);
		}
	}
}
