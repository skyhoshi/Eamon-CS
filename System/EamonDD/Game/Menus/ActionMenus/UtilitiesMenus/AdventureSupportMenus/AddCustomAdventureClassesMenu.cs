
// AddCustomAdventureClassesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCustomAdventureClassesMenu : AdventureSupportMenu01, IAddCustomAdventureClassesMenu
	{
		protected virtual void CheckForMissingClasses()
		{
			RetCode rc;

			var generatedClasses = new string[] { "Artifact", "Effect", "Engine", "GameState", "Hint", "Module", "Monster", "Room" };

			SelectedClasses = new List<string>();

			foreach (var genClass in generatedClasses)
			{
				var origFileName = AdvTemplateDir + @"\Adventures\YourAdventureName\Game\" + genClass + ".cs";

				var gameFileName = Constants.AdventuresDir + @"\" + AdventureName + @"\Game\" + genClass + ".cs";

				if (!Globals.File.Exists(gameFileName))
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Print("The {0} class appears to be missing.", AdventureName + ".Game." + genClass);

					Globals.Out.Write("{0}Would you like to add it (Y/N): ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Buf.Length > 0 && Buf[0] == 'Y')
					{
						SelectedClasses.Add(genClass);
					}
				}
			}

			if (SelectedClasses.Count == 0)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The custom adventure library (.dll) has no missing generated classes.");

				GotoCleanup = true;
			}
		}

		protected virtual void AddSelectedClasses()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.WriteLine();

			foreach (var selectedClass in SelectedClasses)
			{
				var fileText = string.Empty;

				if (string.Equals(selectedClass, "GameState", StringComparison.OrdinalIgnoreCase))
				{
					fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Framework\IGameState.cs");

					Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\IGameState.cs", ReplaceMacros(fileText));
				}

				fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\Game\" + selectedClass + ".cs");

				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\" + selectedClass + ".cs", ReplaceMacros(fileText));
			}
		}

		public override void Execute()
		{
			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("ADD CUSTOM ADVENTURE CLASSES", true);

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

			CheckForMissingClasses();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			QueryToProcessAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			AddSelectedClasses();

			UpdateXmlFileClasses();

			DeleteAdvBinaryFiles();

			RebuildSolution();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintAdventureProcessed();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback classes delete if possible
			}

			Globals.Directory.SetCurrentDirectory(workDir);
		}

		public AddCustomAdventureClassesMenu()
		{

		}
	}
}
