
// DeleteAdventureMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using System.Runtime.InteropServices;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteAdventureMenu : AdventureSupportMenu01, IDeleteAdventureMenu
	{
		protected virtual void RemoveProjectFromSolution()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.WriteLine();

			LoadVsaAssemblyIfNecessary();

			GetVsaObjectIfNecessary();

			if (VsaObject != null)
			{
				var projName = Globals.Path.GetFullPath(Globals.Path.Combine(Constants.AdventuresDir + @"\" + AdventureName, AdventureName + ".csproj"));

				VsaObject.RemoveProjectFromSolution(projName);

				VsaObject.Shutdown();
			}
			else
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not deleted.");

				GotoCleanup = true;
			}
		}

		public override void Execute()
		{
			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("DELETE ADVENTURE", true);

			Debug.Assert(!Globals.Engine.IsAdventureFilesetLoaded());

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
