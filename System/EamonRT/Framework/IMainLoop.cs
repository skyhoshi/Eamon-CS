
// IMainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;

namespace EamonRT.Framework
{
	public interface IMainLoop
	{
		StringBuilder Buf { get; set; }

		bool ShouldStartup { get; set; }

		bool ShouldShutdown { get; set; }

		bool ShouldExecute { get; set; }

		void Startup();

		void Shutdown();

		void Execute();
	}
}
