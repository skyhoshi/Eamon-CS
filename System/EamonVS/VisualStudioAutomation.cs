
// VisualStudioAutomation.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Automation;

namespace EamonVS
{
	public class VisualStudioAutomation : IVisualStudioAutomation
	{
		public virtual string SolutionFile { get; set; }

		public virtual RetCode AddProjectToSolution(string projName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(projName));

			try
			{
				using (var process = new Process())
				{
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.StartInfo.FileName = "dotnet";
					process.StartInfo.Arguments = string.Format("sln Eamon.Adventures.sln add {0} --in-root", projName);		// --in-root requires .NET Core 3.0+ SDK
					process.StartInfo.WorkingDirectory = string.Format("..{0}..", Path.DirectorySeparatorChar);

					Console.Out.Write("Adding {0} project... ", Path.GetFileNameWithoutExtension(projName));

					process.Start();
					process.WaitForExit();		// May want timeout

					result = process.ExitCode == 0 ? RetCode.Success : RetCode.Failure;		// Verify this

					if (result == RetCode.Success)
					{
						Console.Out.WriteLine("succeeded");
					}
					else
					{
						Console.Out.WriteLine("failed");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());

				result = RetCode.Failure;
			}

			return result;
		}

		public virtual RetCode RemoveProjectFromSolution(string projName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(projName));

			try
			{
				using (var process = new Process())
				{
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.StartInfo.FileName = "dotnet";
					process.StartInfo.Arguments = string.Format("sln Eamon.Adventures.sln remove {0}", projName);
					process.StartInfo.WorkingDirectory = string.Format("..{0}..", Path.DirectorySeparatorChar);

					Console.Out.Write("Removing {0} project... ", Path.GetFileNameWithoutExtension(projName));

					process.Start();
					process.WaitForExit();		// May want timeout

					result = process.ExitCode == 0 ? RetCode.Success : RetCode.Failure;		// Verify this

					if (result == RetCode.Success)
					{
						Console.Out.WriteLine("succeeded");
					}
					else
					{
						Console.Out.WriteLine("failed");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());

				result = RetCode.Failure;
			}

			return result;
		}

		public virtual RetCode RebuildProject(string projName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(projName));

			try
			{
				using (var process = new Process())
				{
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.StartInfo.FileName = "dotnet";
					process.StartInfo.Arguments = string.Format("build {0}", projName);
					process.StartInfo.WorkingDirectory = string.Format("..{0}..", Path.DirectorySeparatorChar);

					Console.Out.Write("Rebuilding {0} project... ", Path.GetFileNameWithoutExtension(projName));

					process.Start();
					process.WaitForExit();		// May want timeout

					result = process.ExitCode == 0 ? RetCode.Success : RetCode.Failure;     // Verify this

					if (result == RetCode.Success)
					{
						Console.Out.WriteLine("succeeded");
					}
					else
					{
						Console.Out.WriteLine("failed");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());

				result = RetCode.Failure;
			}

			return result;
		}

		public virtual RetCode Shutdown()
		{
			RetCode result;

			try
			{
				result = RetCode.Success;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());

				result = RetCode.Failure;
			}

			return result;
		}
	}
}
