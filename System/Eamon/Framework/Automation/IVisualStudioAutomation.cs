
// IVisualStudioAutomation.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Automation
{
	/// <summary></summary>
	public interface IVisualStudioAutomation
	{
		/// <summary></summary>
		string SolutionFile { get; set; }

		/// <summary></summary>
		/// <param name="projName"></param>
		/// <returns></returns>
		RetCode AddProjectToSolution(string projName);

		/// <summary></summary>
		/// <param name="projName"></param>
		/// <returns></returns>
		RetCode RemoveProjectFromSolution(string projName);

		/// <summary></summary>
		/// <param name="projName"></param>
		/// <returns></returns>
		RetCode RebuildProject(string projName);

		/// <summary></summary>
		/// <returns></returns>
		RetCode Shutdown();
	}
}
