
// AddCustomAdventureClassesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCustomAdventureClassesMenu : AdventureSupportMenu01, IAddCustomAdventureClassesMenu
	{
		protected virtual IList<bool> IncludeInterfaces { get; set; }

		protected virtual void SelectClassFilesToAdd()
		{
			var invalidClassFileNames = new string[] { "Program.cs", "Engine.cs", "IPluginClassMappings.cs", "IPluginConstants.cs", "IPluginGlobals.cs", "PluginClassMappings.cs", "PluginConstants.cs", "PluginContext.cs", "PluginGlobals.cs" };

			SelectedClassFiles = new List<string>();

			IncludeInterfaces = new List<bool>();

			var classFileName = string.Empty;

			var includeInterface = false;

			while (true)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Enter file name of interface/class: ", Environment.NewLine);

				Buf.Clear();

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, 120, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				classFileName = Buf.Trim().ToString().Replace('/', '\\');

				if (classFileName.Length == 0)
				{
					goto Cleanup;
				}

				includeInterface = false;

				if (!classFileName.StartsWith(@".\Eamon\") && !classFileName.StartsWith(@".\EamonDD\") && !classFileName.StartsWith(@".\EamonRT\"))
				{
					classFileName = string.Empty;
				}
				else if (!classFileName.Contains(@"\Game\") && !classFileName.Contains(@"\Framework\"))
				{
					classFileName = string.Empty;
				}
				else 
				{
					var destClassFileName = classFileName.Replace(classFileName.StartsWith(@".\Eamon\") ? @".\Eamon\" : classFileName.StartsWith(@".\EamonDD\") ? @".\EamonDD\" : @".\EamonRT\", Constants.AdventuresDir + @"\" + AdventureName + @"\").Replace(@"..\..\", @"..\");

					if (!classFileName.EndsWith(".cs") || classFileName.Contains(@"\.") || invalidClassFileNames.FirstOrDefault(fn => string.Equals(fn, Globals.Path.GetFileName(classFileName), StringComparison.OrdinalIgnoreCase)) != null || SelectedClassFiles.FirstOrDefault(fn => string.Equals(fn, classFileName, StringComparison.OrdinalIgnoreCase)) != null || Globals.File.Exists(destClassFileName))
					{
						classFileName = string.Empty;
					}

					if (!Globals.File.Exists(classFileName))
					{
						if (classFileName.StartsWith(@".\EamonRT\Game\States\") || classFileName.StartsWith(@".\EamonRT\Game\Commands\") || classFileName.StartsWith(@".\EamonRT\Framework\States\") || classFileName.StartsWith(@".\EamonRT\Framework\Commands\"))
						{
							Globals.Out.Print("{0}", Globals.LineSep);

							Globals.Out.Write("{0}Would you like to derive directly from {1} (Y/N) [N]: ", Environment.NewLine, 
								classFileName.Contains(@"\Game\States\") ? "State" :	
								classFileName.Contains(@"\Game\Commands\") ? "Command" :	
								classFileName.Contains(@"\Framework\States\") ? "IState" : 
								"ICommand");

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "N", Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								if (classFileName.Contains(@"\Game\"))
								{
									includeInterface = true;
								}
							}
							else
							{
								classFileName = string.Empty;
							}
						}
						else
						{
							classFileName = string.Empty;
						}
					}
				}

				Globals.Out.Print("{0}", Globals.LineSep);

				if (classFileName.Length > 0)
				{
					SelectedClassFiles.Add(classFileName);

					if (!includeInterface && classFileName.Contains(@"\Game\"))
					{
						Globals.Out.Write("{0}Would you like to add a custom interface for this class (Y/N) [N]: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "N", Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'Y')
						{
							includeInterface = true;
						}

						Globals.Out.Print("{0}", Globals.LineSep);
					}

					IncludeInterfaces.Add(includeInterface);

					Globals.Out.Print("The file name path was added to the selected class files list.");
				}
				else
				{
					Globals.Out.Print("The file name path was invalid or the source/destination file was not found, or already exists.");
				}
			}

		Cleanup:

			if (SelectedClassFiles.Count == 0)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not processed.");

				GotoCleanup = true;
			}
		}

		protected virtual void AddSelectedClassFiles()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.WriteLine();

			for (var i = 0; i < SelectedClassFiles.Count; i++)
			{
				ParentClassFileName = SelectedClassFiles[i].Replace(@".\", @"..\");

				IncludeInterface = IncludeInterfaces[i];

				CreateCustomClassFile();
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

			Globals.Directory.SetCurrentDirectory("..");

			SelectClassFilesToAdd();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			Globals.Directory.SetCurrentDirectory(workDir);

			QueryToProcessAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			AddSelectedClassFiles();

			Globals.Directory.SetCurrentDirectory(Constants.AdventuresDir + @"\" + AdventureName);

			UpdateXmlFileClasses();

			Globals.Directory.SetCurrentDirectory(workDir);

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
