
// VisualStudioAutomation.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using Eamon;
using Eamon.Framework.Automation;

// Full credit:  https://www.helixoft.com/blog/creating-envdte-dte-for-vs-2017-from-outside-of-the-devenv-exe.html

// Note:  always remove the EamonVS project from the Eamon.Desktop solution prior to stepping through code in the VisualStudioAutomation class

namespace EamonVS
{
	public class VisualStudioAutomation : IVisualStudioAutomation
	{
		protected virtual EnvDTE.DTE Dte { get; set; }

		protected virtual EnvDTE.Solution Solution { get; set; }

		public virtual string DevenvExePath { get; set; }

		public virtual string SolutionFile { get; set; }

		/// <summary>Creates and returns a DTE instance of specified VS version.</summary>
		/// <param name="devenvPath">The full path to the devenv.exe.
		/// <returns>DTE instance or <see langword="null"> if not found.</see></returns>
		protected virtual EnvDTE.DTE CreateDteInstance()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(DevenvExePath));

			EnvDTE.DTE dte = null;
			Process proc = null;

			// start devenv
			ProcessStartInfo procStartInfo = new ProcessStartInfo();
			procStartInfo.Arguments = "-Embedding";
			procStartInfo.CreateNoWindow = true;
			procStartInfo.FileName = DevenvExePath;
			procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(DevenvExePath);

			try
			{
				proc = Process.Start(procStartInfo);
			}
			catch (Exception)
			{
				return null;
			}

			if (proc == null)
			{
				return null;
			}

			// get DTE
			dte = GetDTE(proc.Id, 120);

			return dte;
		}

		/// <summary>
		/// Gets the DTE object from any devenv process.
		/// </summary>
		/// <remarks>
		/// After starting devenv.exe, the DTE object is not ready. We need to try repeatedly and fail after the
		/// timeout.
		/// </remarks>
		/// <param name="processId">
		/// <param name="timeout">Timeout in seconds.
		/// <returns>
		/// Retrieved DTE object or <see langword="null"> if not found.
		/// </see></returns>
		protected virtual EnvDTE.DTE GetDTE(int processId, int timeout)
		{
			EnvDTE.DTE res = null;
			DateTime startTime = DateTime.Now;

			while (res == null && DateTime.Now.Subtract(startTime).Seconds < timeout)
			{
				Thread.Sleep(1000);
				res = GetDTE(processId);
			}

			return res;
		}

		[DllImport("ole32.dll")]
		protected static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

		/// <summary>
		/// Gets the DTE object from any devenv process.
		/// </summary>
		/// <param name="processId">
		/// <returns>
		/// Retrieved DTE object or <see langword="null"> if not found.
		/// </see></returns>
		protected virtual EnvDTE.DTE GetDTE(int processId)
		{
			object runningObject = null;

			IBindCtx bindCtx = null;
			IRunningObjectTable rot = null;
			IEnumMoniker enumMonikers = null;

			try
			{
				Marshal.ThrowExceptionForHR(CreateBindCtx(reserved: 0, ppbc: out bindCtx));
				bindCtx.GetRunningObjectTable(out rot);
				rot.EnumRunning(out enumMonikers);

				IMoniker[] moniker = new IMoniker[1];
				IntPtr numberFetched = IntPtr.Zero;
				while (enumMonikers.Next(1, moniker, numberFetched) == 0)
				{
					IMoniker runningObjectMoniker = moniker[0];

					string name = null;

					try
					{
						if (runningObjectMoniker != null)
						{
							runningObjectMoniker.GetDisplayName(bindCtx, null, out name);
						}
					}
					catch (UnauthorizedAccessException)
					{
						// Do nothing, there is something in the ROT that we do not have access to.
					}

					Regex monikerRegex = new Regex(@"!VisualStudio.DTE\.\d+\.\d+\:" + processId, RegexOptions.IgnoreCase);
					if (!string.IsNullOrEmpty(name) && monikerRegex.IsMatch(name))
					{
						Marshal.ThrowExceptionForHR(rot.GetObject(runningObjectMoniker, out runningObject));
						break;
					}
				}
			}
			finally
			{
				if (enumMonikers != null)
				{
					Marshal.ReleaseComObject(enumMonikers);
				}

				if (rot != null)
				{
					Marshal.ReleaseComObject(rot);
				}

				if (bindCtx != null)
				{
					Marshal.ReleaseComObject(bindCtx);
				}
			}

			return runningObject as EnvDTE.DTE;
		}

		protected virtual RetCode ExecuteWithRetry(Func<bool> func, long numTries, long sleepMs)
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

				Thread.Sleep(result ? 1000 : (int)sleepMs);

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

		protected virtual RetCode CreateDteIfNecessary()
		{
			RetCode result;

			try
			{
				result = RetCode.Success;

				if (Dte == null)
				{
					Console.Out.Write("Creating Visual Studio DTE... ");

					// create DTE
					result = ExecuteWithRetry(() => 
					{
						Dte = CreateDteInstance();
						return Dte != null;
					}, 50, 250);

					if (result == RetCode.Success && Dte != null)
					{
						result = ExecuteWithRetry(() => 
						{
							Dte.UserControl = false;
							return true;
						}, 50, 250);
					}

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

				Dte = null;

				result = RetCode.Failure;
			}

			return result;
		}

		protected virtual RetCode OpenSolutionIfNecessary()
		{
			RetCode result;

			Debug.Assert(Dte != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(SolutionFile));

			try
			{
				result = RetCode.Success;

				if (Solution == null)
				{
					Console.Out.Write("Opening {0} solution... ", Path.GetFileNameWithoutExtension(SolutionFile));

					result = ExecuteWithRetry(() =>
					{
						Solution = Dte.Solution;
						return Solution != null;
					}, 50, 250);

					if (result == RetCode.Success && Solution != null)
					{
						result = ExecuteWithRetry(() =>
						{
							Solution.Open(SolutionFile);
							return true;
						}, 50, 250);
					}

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

				Solution = null;

				result = RetCode.Failure;
			}

			return result;
		}

		public virtual RetCode AddProjectToSolution(string projName)
		{
			RetCode result;

			Debug.Assert(!string.IsNullOrWhiteSpace(projName));

			try
			{
				result = CreateDteIfNecessary();

				if (result == RetCode.Success && Dte != null)
				{
					result = OpenSolutionIfNecessary();

					if (result == RetCode.Success && Solution != null)
					{
						Console.Out.Write("Adding {0} project... ", Path.GetFileNameWithoutExtension(projName));

						result = ExecuteWithRetry(() =>
						{
							Solution.AddFromFile(projName);
							return true;
						}, 50, 250);

						if (result == RetCode.Success)
						{
							Console.Out.WriteLine("succeeded");

							Console.Out.Write("Saving solution... ");

							result = ExecuteWithRetry(() =>
							{
								Solution.SaveAs(SolutionFile);
								return true;
							}, 50, 250);

							if (result == RetCode.Success)
							{
								Console.Out.WriteLine("succeeded");
							}
							else
							{
								Console.Out.WriteLine("failed");
							}
						}
						else
						{
							Console.Out.WriteLine("failed");
						}
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
				result = CreateDteIfNecessary();

				if (result == RetCode.Success && Dte != null)
				{
					result = OpenSolutionIfNecessary();

					if (result == RetCode.Success && Solution != null)
					{
						Console.Out.Write("Removing {0} project... ", Path.GetFileNameWithoutExtension(projName));

						result = ExecuteWithRetry(() =>
						{
							for (var i = 1; i <= Solution.Projects.Count; i++)
							{
								var proj = Solution.Projects.Item(i);

								if (string.Equals(proj.Name, Path.GetFileNameWithoutExtension(projName), StringComparison.OrdinalIgnoreCase))
								{
									Solution.Remove(proj);
									goto Cleanup;
								} 
							}
						Cleanup:
							return true;
						}, 50, 250);

						if (result == RetCode.Success)
						{
							Console.Out.WriteLine("succeeded");

							Console.Out.Write("Saving solution... ");

							result = ExecuteWithRetry(() =>
							{
								Solution.SaveAs(SolutionFile);
								return true;
							}, 50, 250);

							if (result == RetCode.Success)
							{
								Console.Out.WriteLine("succeeded");
							}
							else
							{
								Console.Out.WriteLine("failed");
							}
						}
						else
						{
							Console.Out.WriteLine("failed");
						}
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

		public virtual RetCode RebuildSolution()
		{
			RetCode result;

			try
			{
				result = CreateDteIfNecessary();

				if (result == RetCode.Success && Dte != null)
				{
					result = OpenSolutionIfNecessary();

					if (result == RetCode.Success && Solution != null)
					{
						Console.Out.Write("Rebuilding solution... ");

						result = ExecuteWithRetry(() =>
						{
							Dte.ExecuteCommand("Build.CleanSolution");
							return true;
						}, 50, 250);

						if (result == RetCode.Success)
						{
							result = ExecuteWithRetry(() => Solution.SolutionBuild.BuildState == EnvDTE.vsBuildState.vsBuildStateDone, 1200, 250);

							if (result == RetCode.Success)
							{
								if (Solution.SolutionBuild.BuildState == EnvDTE.vsBuildState.vsBuildStateDone)
								{
									result = ExecuteWithRetry(() =>
									{
										Dte.ExecuteCommand("Build.RebuildSolution");
										return true;
									}, 50, 250);

									if (result == RetCode.Success)
									{
										result = ExecuteWithRetry(() => Solution.SolutionBuild.BuildState == EnvDTE.vsBuildState.vsBuildStateDone, 1200, 250);

										if (result == RetCode.Success)
										{
											if (Solution.SolutionBuild.BuildState == EnvDTE.vsBuildState.vsBuildStateDone)
											{
												Console.Out.WriteLine("succeeded");
											}
											else
											{
												Console.Out.WriteLine("timed out");

												result = RetCode.Failure;
											}
										}
										else
										{
											Console.Out.WriteLine("failed");
										}
									}
									else
									{
										Console.Out.WriteLine("failed");
									}
								}
								else
								{
									Console.Out.WriteLine("timed out");

									result = RetCode.Failure;
								}
							}
							else
							{
								Console.Out.WriteLine("failed");
							}
						}
						else
						{
							Console.Out.WriteLine("failed");
						}
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

				if (Solution != null)
				{
					result = ExecuteWithRetry(() =>
					{
						Solution.Close();
						return true;
					}, 50, 250);

					Solution = null;
				}

				if (result == RetCode.Success && Dte != null)
				{
					result = ExecuteWithRetry(() =>
					{
						Dte.Quit();
						return true;
					}, 50, 250);

					Dte = null;
				}
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
