
// IVisualStudioAutomation.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Automation
{
	public interface IVisualStudioAutomation
	{
		string DevenvExePath { get; set; }

		string SolutionFile { get; set; }

		RetCode AddProjectToSolution(string projName);

		RetCode RemoveProjectFromSolution(string projName);

		RetCode RebuildSolution(string libraryName);

		RetCode Shutdown();
	}
}
