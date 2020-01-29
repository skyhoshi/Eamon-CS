
// DeleteAdventureMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Eamon;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteAdventureMenu : AdventureSupportMenu01, IDeleteAdventureMenu
	{
		/// <summary></summary>
		protected virtual void QueryToDeleteAdventure()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("WARNING:  you are about to delete this adventure and all associated textfiles from storage.  If you have any doubts, you should select 'N' and backup your Eamon CS repository before proceeding.  This action is PERMANENT!");

			gOut.Write("{0}Would you like to delete this adventure from Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not deleted.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		protected virtual void RemoveProjectFromSolution()
		{
			var result = RetCode.Failure;

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

			LoadVsaAssemblyIfNecessary();

			GetVsaObjectIfNecessary();

			if (VsaObject != null)
			{
				if (IsAdventureNameValid())
				{
					var projName = Globals.Path.GetFullPath(Globals.Path.Combine(Constants.AdventuresDir + @"\" + AdventureName, AdventureName + ".csproj"));

					VsaObject.RemoveProjectFromSolution(projName);

					result = RetCode.Success;
				}

				VsaObject.Shutdown();
			}

			if (result == RetCode.Failure)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not deleted.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		protected virtual void DeleteAdventureFolder()
		{
			if (IsAdventureNameValid())
			{
				if (Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
				{
					Globals.Directory.Delete(Constants.AdventuresDir + @"\" + AdventureName, true);
				}

				if (Globals.Directory.Exists(Constants.AndroidAdventuresDir + @"\" + AdventureName))
				{
					Globals.Directory.Delete(Constants.AndroidAdventuresDir + @"\" + AdventureName, true);
				}
			}
		}

		/// <summary></summary>
		protected virtual void DeleteQuickLaunchFiles()
		{
			// Note: QuickLaunch files missing in Eamon CS Mobile

			if (IsAdventureNameValid())
			{
				var srcFileName = Constants.QuickLaunchDir + @"\Unix\EamonDD\Edit" + AdventureName + ".sh";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}

				srcFileName = Constants.QuickLaunchDir + @"\Unix\EamonRT\Resume" + AdventureName + ".sh";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}

				srcFileName = Constants.QuickLaunchDir + @"\Windows\EamonDD\Edit" + AdventureName + ".bat";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}

				srcFileName = Constants.QuickLaunchDir + @"\Windows\EamonRT\Resume" + AdventureName + ".bat";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}
			}
		}

		/// <summary></summary>
		protected virtual void PrintAdventureDeleted()
		{
			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("The adventure was successfully deleted.");
		}

		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("DELETE ADVENTURE", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			var workDir = Globals.Directory.GetCurrentDirectory();

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			SelectAdvDbTextFiles();

			QueryToDeleteAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (IsAdventureNameValid())
			{
				if (Globals.File.Exists(Constants.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj"))
				{
					if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					{
						CheckForPrerequisites();

						if (GotoCleanup)
						{
							goto Cleanup;
						}
					}

					DeleteAdvBinaryFiles();

					if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					{
						RemoveProjectFromSolution();
					}
				}

				UpdateAdvDbTextFiles();
			}

			DeleteAdventureFolder();

			DeleteQuickLaunchFiles();

			PrintAdventureDeleted();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback adventure delete if possible
			}

			Globals.Directory.SetCurrentDirectory(workDir);
		}

		public DeleteAdventureMenu()
		{

		}
	}
}
