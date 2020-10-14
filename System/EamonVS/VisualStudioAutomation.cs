
// VisualStudioAutomation.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Threading;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Automation;

namespace EamonVS
{
	public class VisualStudioAutomation : IVisualStudioAutomation
	{
		public virtual string DevenvExePath { get; set; }

		public virtual string SolutionFile { get; set; }

		public virtual RetCode AddProjectToSolution(string projName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(projName));

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

		public virtual RetCode RemoveProjectFromSolution(string projName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(projName));

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

		public virtual RetCode RebuildSolution(string libraryName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(libraryName));

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

		/// <summary></summary>
		/// <param name="func"></param>
		/// <param name="numTries"></param>
		/// <param name="sleepMs"></param>
		/// <returns></returns>
		public virtual RetCode ExecuteWithRetry(Func<bool> func, long numTries, long sleepMs)
		{
			Debug.Assert(func != null);

			Exception savedEx = null;

			bool result = false;

			for (var i = 0; i < numTries; i++)
			{
				try
				{
					result = func();
				}
				catch (Exception ex)
				{
					if (savedEx == null)
					{
						savedEx = ex;
					}

					result = false;
				}

				Thread.Sleep(result ? 500 : (int)sleepMs);

				if (result)
				{
					break;
				}
			}

			if (!result && savedEx != null)
			{
				throw new Exception("ExecuteWithRetry: caught exception", savedEx);
			}

			return result ? RetCode.Success : RetCode.Failure;
		}
	}
}
