
// AddCustomAdventureMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCustomAdventureMenu : AdventureSupportMenu01, IAddCustomAdventureMenu
	{
		protected virtual void CopyCustomFiles()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin");

			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin");

			var fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\ChangeLog.txt");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\ChangeLog.txt", ReplaceMacros(fileText));

			fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Program.cs");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Program.cs", ReplaceMacros(fileText));

			fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\YourAdventureName.csproj");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj", ReplaceMacros(fileText));

			var fileNames = new string[] { "IPluginClassMappings.cs", "IPluginConstants.cs", "IPluginGlobals.cs" };

			foreach (var fileName in fileNames)
			{
				fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Framework\Plugin\" + fileName);

				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin\" + fileName, ReplaceMacros(fileText));
			}

			fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Framework\IGameState.cs");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\IGameState.cs", ReplaceMacros(fileText));

			fileNames = new string[] { "PluginClassMappings.cs", "PluginConstants.cs", "PluginContext.cs", "PluginGlobals.cs" };

			foreach (var fileName in fileNames)
			{
				fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Game\Plugin\" + fileName);

				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin\" + fileName, ReplaceMacros(fileText));
			}

			fileNames = new string[] { "Artifact.cs", "Effect.cs", "Engine.cs", "GameState.cs", "Hint.cs", "Module.cs", "Monster.cs", "Room.cs" };

			foreach (var fileName in fileNames)
			{
				fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Game\" + fileName);

				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\" + fileName, ReplaceMacros(fileText));
			}
		}

		protected virtual void AddProjectToSolution()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.WriteLine();

			LoadVsaAssemblyIfNecessary();

			GetVsaObjectIfNecessary();

			if (VsaObject != null)
			{
				var projName = Globals.Path.GetFullPath(Globals.Path.Combine(Constants.AdventuresDir + @"\" + AdventureName, AdventureName + ".csproj"));

				VsaObject.AddProjectToSolution(projName);
			}
			else
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		public override void Execute()
		{
			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("ADD CUSTOM ADVENTURE", true);

			Debug.Assert(!Globals.Engine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			var workDir = Globals.Directory.GetCurrentDirectory();

			CheckForPrerequisites();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAuthorName();

			GetAuthorInitials();

			SelectAdvDbTextFiles();

			QueryToAddAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			CopyQuickLaunchFiles();

			CreateAdventureFolder();

			CopyHintsXml();

			CopyCustomFiles();

			UpdateAdvDbTextFiles();

			AddProjectToSolution();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			RebuildSolution();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintAdventureCreated();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback adventure buildout if necessary
			}

			Globals.Directory.SetCurrentDirectory(workDir);
		}

		public AddCustomAdventureMenu()
		{

		}
	}
}
