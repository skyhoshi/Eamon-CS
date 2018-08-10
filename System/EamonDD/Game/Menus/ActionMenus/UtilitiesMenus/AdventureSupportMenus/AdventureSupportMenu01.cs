
// AdventureSupportMenu01.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Automation;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AdventureSupportMenu01 : Menu, IAdventureSupportMenu01
	{
		protected virtual bool GotoCleanup { get; set; }

		protected virtual bool AddAdventure { get; set; }

		protected virtual string AdventureName { get; set; }

		protected virtual string AdventureName01 { get; set; }

		protected virtual string AuthorName { get; set; }

		protected virtual string AuthorInitials { get; set; }

		protected virtual string AdvTemplateDir { get; set; }

		protected virtual IList<string> SelectedAdvDbTextFiles { get; set; }

		protected virtual Assembly VsaAssembly { get; set; }

		protected virtual IVisualStudioAutomation VsaObject { get; set; }

		protected virtual string ReplaceMacros(string fileText)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fileText));

			return fileText.Replace("YourAdventureName", AdventureName).Replace("YourAuthorName", AuthorName).Replace("YourAuthorInitials", AuthorInitials);
		}

		protected virtual void LoadVsaAssemblyIfNecessary()
		{
			if (VsaAssembly == null)
			{
				VsaAssembly = Assembly.LoadFrom(Globals.Path.GetFullPath(@".\EamonVS.dll"));
			}
		}

		protected virtual void GetVsaObjectIfNecessary()
		{
			if (VsaAssembly != null && VsaObject == null)
			{
				var type = VsaAssembly.GetType("EamonVS.VisualStudioAutomation");

				if (type != null)
				{
					VsaObject = (IVisualStudioAutomation)Activator.CreateInstance(type);

					if (VsaObject != null)
					{
						VsaObject.DevenvExePath = Globals.DevenvExePath;

						VsaObject.SolutionFile = Globals.Path.GetFullPath(Constants.EamonDesktopSlnFile);
					}
				}
			}
		}

		protected virtual void CheckForPrerequisites()
		{
			if (!Globals.File.Exists(Globals.DevenvExePath))
			{
				if (!AddAdventure)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}

				Globals.Out.Print("Could not locate the Visual Studio 2017 devenv.exe program at the following path:");

				Globals.Out.WordWrap = false;

				Globals.Out.Print(Globals.DevenvExePath);

				Globals.Out.WordWrap = true;

				Globals.Out.Print("You may need to modify the LoadAdventureSupportMenu .bat or .sh file to use the -dep flag.  See the documentation section on creating custom adventures for more details.");

				GotoCleanup = true;
			}
			else if (!Globals.File.Exists(Globals.Path.GetFullPath(@".\EamonVS.dll")))
			{
				if (!AddAdventure)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}

				Globals.Out.Print("Could not locate the Eamon CS EamonVS.dll library at the following path:");

				Globals.Out.WordWrap = false;

				Globals.Out.Print(Globals.Path.GetFullPath(@".\EamonVS.dll"));

				Globals.Out.WordWrap = true;

				Globals.Out.Print("You may need to compile it using the Eamon.Desktop solution and Visual Studio 2017.");

				GotoCleanup = true;
			}
			else if (!Globals.File.Exists(Globals.Path.GetFullPath(@".\envdte.dll")))
			{
				if (!AddAdventure)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}

				Globals.Out.Print("Could not locate the Visual Studio 2017 envdte.dll library at the following path:");

				Globals.Out.WordWrap = false;

				Globals.Out.Print(Globals.Path.GetFullPath(@".\envdte.dll"));

				Globals.Out.WordWrap = true;

				Globals.Out.Print(@"This library is copied into System\Bin when the EamonVS project is compiled using the Eamon.Desktop solution and Visual Studio 2017.");

				GotoCleanup = true;
			}
			else if (!Globals.File.Exists(Globals.Path.GetFullPath(Constants.EamonDesktopSlnFile)))
			{
				if (!AddAdventure)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}

				Globals.Out.Print("Could not locate the Eamon.Desktop solution at the following path:");

				Globals.Out.WordWrap = false;

				Globals.Out.Print(Globals.Path.GetFullPath(Constants.EamonDesktopSlnFile));

				Globals.Out.WordWrap = true;

				Globals.Out.Print(@"This Eamon CS repository may be compromised since the solution should always be present.");

				GotoCleanup = true;
			}

			if (GotoCleanup)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not {0}.", AddAdventure ? "created" : "deleted");
			}
		}

		protected virtual void GetAdventureName()
		{
			var invalidAdventureNames = new string[] { "Adventures", "Catalog", "Characters", "Contemporary", "Fantasy", "SciFi", "Test", "Workbench", "WorkInProgress", "AdventureSupportMenu", "AdventureTemplates" };

			if (AddAdventure)
			{
				Globals.Out.Print("You must enter a name for your new adventure (eg, The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures.");

				Globals.Out.Print("Note:  the name will be used to produce a shortened form suitable for use as a folder name under the Adventures directory and also as a C# namespace (eg, TheBeginnersCave).");
			}
			else
			{
				Globals.Out.Print("You must enter the name of the adventure you wish to delete (eg, The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures.");
			}

			AdventureName = string.Empty;

			while (AdventureName.Length == 0)
			{
				Globals.Out.Write("{0}Enter the name of the {1}adventure: ", Environment.NewLine, AddAdventure ? "new " : "");

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				AdventureName01 = Buf.Trim().ToString();

				var tempStr = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(AdventureName01);

				AdventureName = new string((from char ch in tempStr where Globals.Engine.IsCharAlnum(ch) select ch).ToArray());

				if (AdventureName.Length > 0 && (Globals.Engine.IsCharDigit(AdventureName[0]) || invalidAdventureNames.FirstOrDefault(n => string.Equals(AdventureName, n, StringComparison.OrdinalIgnoreCase)) != null))
				{
					AdventureName = string.Empty;
				}

				if (AdventureName.Length > Constants.FsFileNameLen - 4)
				{
					AdventureName = AdventureName.Substring(0, Constants.FsFileNameLen - 4);
				}

				if (AdventureName.Length == 0)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}
			}

			if (AddAdventure && Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure already exists.");

				GotoCleanup = true;
			}
			else if (!AddAdventure && !Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure does not exist.");

				GotoCleanup = true;
			}
		}

		protected virtual void GetAuthorName()
		{
			AuthorName = string.Empty;

			while (AuthorName.Length == 0)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Enter the name(s) of the adventure's Eamon CS author(s): ", Environment.NewLine);

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				AuthorName = Buf.Trim().ToString();
			}
		}

		protected virtual void GetAuthorInitials()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}Enter the initials of the adventure's main Eamon CS author: ", Environment.NewLine);

			Buf.Clear();

			var rc = Globals.In.ReadField(Buf, Constants.ModVolLabelLen - 4, null, '_', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharAlpha, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			AuthorInitials = Buf.Trim().ToString();
		}

		protected virtual void SelectAdvDbTextFiles()
		{
			RetCode rc;

			SelectedAdvDbTextFiles = new List<string>();

			var advDbTextFiles = new string[] { "ADVENTURES.XML", "FANTASY.XML", "SCIFI.XML", "CONTEMPORARY.XML", "TEST.XML", "WIP.XML" };

			if (AddAdventure)
			{
				var inputDefaultValue = "Y";

				foreach (var advDbTextFile in advDbTextFiles)
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Write("{0}Add this game to adventure database \"{1}\" (Y/N) [{2}]: ", Environment.NewLine, advDbTextFile, inputDefaultValue);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, inputDefaultValue, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Buf.Length == 0 || Buf[0] != 'N')
					{
						SelectedAdvDbTextFiles.Add(advDbTextFile);

						if (!string.Equals(advDbTextFile, "ADVENTURES.XML", StringComparison.OrdinalIgnoreCase))
						{
							inputDefaultValue = "N";
						}
					}
				}
			}
			else
			{
				SelectedAdvDbTextFiles.AddRange(advDbTextFiles);
			}

			var customAdvDbTextFile = string.Empty;

			while (true)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				if (customAdvDbTextFile.Length == 0)
				{
					Globals.Out.Print("If you would like to {0} one or more custom adventure databases, enter those file names now (eg, HORROR.XML).  To skip this step, or if you are done, just press enter.", AddAdventure ? "add this adventure to" : "delete this adventure from");
				}

				Globals.Out.Write("{0}Enter name of custom adventure database: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharAlnumPeriodUnderscore, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				customAdvDbTextFile = Buf.Trim().ToString();

				if (customAdvDbTextFile.Length > 0)
				{
					if (!SelectedAdvDbTextFiles.Contains(customAdvDbTextFile))
					{
						SelectedAdvDbTextFiles.Add(customAdvDbTextFile);
					}
				}
				else
				{
					break;
				}
			}
		}

		protected virtual void QueryToAddAdventure()
		{
			RetCode rc;

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}Would you like to add this adventure to Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		protected virtual void QueryToDeleteAdventure()
		{
			RetCode rc;

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("WARNING:  you are about to delete this adventure and all associated textfiles from storage.  If you have any doubts, you should select 'N' and backup your Eamon CS repository before proceeding.  This action is PERMANENT!");

			Globals.Out.Write("{0}Would you like to delete this adventure from Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not deleted.");

				GotoCleanup = true;
			}
		}

		protected virtual void CopyQuickLaunchFiles()
		{
			var fileText = string.Empty;

			// Note: QuickLaunch files missing in Eamon CS Mobile

			var srcFileName = AdvTemplateDir + @"\QuickLaunch\Unix\EamonDD\EditYourAdventureName.sh";

			var destFileName = Constants.QuickLaunchDir + @"\Unix\EamonDD\Edit" + AdventureName + ".sh";

			if (Globals.File.Exists(srcFileName))
			{
				fileText = Globals.File.ReadAllText(srcFileName);

				Globals.File.WriteAllText(destFileName, ReplaceMacros(fileText));
			}

			srcFileName = AdvTemplateDir + @"\QuickLaunch\Unix\EamonRT\ResumeYourAdventureName.sh";

			destFileName = Constants.QuickLaunchDir + @"\Unix\EamonRT\Resume" + AdventureName + ".sh";

			if (Globals.File.Exists(srcFileName))
			{
				fileText = Globals.File.ReadAllText(srcFileName);

				Globals.File.WriteAllText(destFileName, ReplaceMacros(fileText));
			}

			srcFileName = AdvTemplateDir + @"\QuickLaunch\Windows\EamonDD\EditYourAdventureName.bat";

			destFileName = Constants.QuickLaunchDir + @"\Windows\EamonDD\Edit" + AdventureName + ".bat";

			if (Globals.File.Exists(srcFileName))
			{
				fileText = Globals.File.ReadAllText(srcFileName);

				Globals.File.WriteAllText(destFileName, ReplaceMacros(fileText));
			}

			srcFileName = AdvTemplateDir + @"\QuickLaunch\Windows\EamonRT\ResumeYourAdventureName.bat";

			destFileName = Constants.QuickLaunchDir + @"\Windows\EamonRT\Resume" + AdventureName + ".bat";

			if (Globals.File.Exists(srcFileName))
			{
				fileText = Globals.File.ReadAllText(srcFileName);

				Globals.File.WriteAllText(destFileName, ReplaceMacros(fileText));
			}
		}

		protected virtual void DeleteQuickLaunchFiles()
		{
			// Note: QuickLaunch files missing in Eamon CS Mobile

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

		protected virtual void CreateAdventureFolder()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName);
		}

		protected virtual void DeleteAdventureFolder()
		{
			if (Globals.Directory.Exists(@"..\..\Adventures\" + AdventureName))
			{
				Globals.Directory.Delete(@"..\..\Adventures\" + AdventureName, true);
			}

			if (Globals.Directory.Exists(Constants.AndroidAdventuresDir + @"\" + AdventureName))
			{
				Globals.Directory.Delete(Constants.AndroidAdventuresDir + @"\" + AdventureName, true);
			}
		}

		protected virtual void CopyHintsXml()
		{
			var fileText = Globals.File.ReadAllText(AdvTemplateDir + @"\Adventures\YourAdventureName\HINTS.XML");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\HINTS.XML", ReplaceMacros(fileText));
		}

		protected virtual void UpdateAdvDbTextFiles()
		{
			RetCode rc;

			foreach (var advDbTextFile in SelectedAdvDbTextFiles)
			{
				rc = Globals.PushDatabase();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var fsfn = Globals.Path.Combine(".", advDbTextFile);

				rc = Globals.Database.LoadFilesets(fsfn, printOutput: false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (AddAdventure)
				{
					var fileset = Globals.CreateInstance<IFileset>(x =>
					{
						x.Uid = Globals.Database.GetFilesetUid();

						x.IsUidRecycled = true;

						x.Name = AdventureName01;

						x.WorkDir = @"..\..\Adventures\" + AdventureName;

						x.PluginFileName = this is IAddStandardAdventureMenu ? "EamonRT.dll" : AdventureName + ".dll";

						x.ConfigFileName = "NONE";

						x.FilesetFileName = "NONE";

						x.CharacterFileName = "NONE";

						x.ModuleFileName = "MODULE.XML";

						x.RoomFileName = "ROOMS.XML";

						x.ArtifactFileName = "ARTIFACTS.XML";

						x.EffectFileName = "EFFECTS.XML";

						x.MonsterFileName = "MONSTERS.XML";

						x.HintFileName = "HINTS.XML";

						x.GameStateFileName = "NONE";
					});

					rc = Globals.Database.AddFileset(fileset);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					var fileset = Globals.Database.FilesetTable.Records.FirstOrDefault(fs => string.Equals(fs.WorkDir, @"..\..\Adventures\" + AdventureName, StringComparison.OrdinalIgnoreCase));

					if (fileset != null)
					{
						Globals.Database.RemoveFileset(fileset.Uid);

						fileset.Dispose();
					}
				}

				rc = Globals.Database.SaveFilesets(fsfn, printOutput: false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				rc = Globals.PopDatabase();

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}
		}

		protected virtual void RebuildSolution()
		{
			LoadVsaAssemblyIfNecessary();

			GetVsaObjectIfNecessary();

			if (VsaObject != null)
			{
				VsaObject.RebuildSolution();

				VsaObject.Shutdown();
			}
			else
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		protected virtual void DeleteAdvBinaryFiles()
		{
			var srcFileName = @".\" + AdventureName + ".dll";

			if (Globals.File.Exists(srcFileName))
			{
				Globals.File.Delete(srcFileName);
			}

			srcFileName = @".\" + AdventureName + ".pdb";

			if (Globals.File.Exists(srcFileName))
			{
				Globals.File.Delete(srcFileName);
			}

			srcFileName = @".\" + AdventureName + ".deps.json";

			if (Globals.File.Exists(srcFileName))
			{
				Globals.File.Delete(srcFileName);
			}
		}

		protected virtual void PrintAdventureCreated()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("The adventure was successfully created.");
		}

		protected virtual void PrintAdventureDeleted()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("The adventure was successfully deleted.");
		}

		public AdventureSupportMenu01()
		{
			Buf = Globals.Buf;

			AddAdventure = this is IAddStandardAdventureMenu || this is IAddCustomAdventureMenu;

			AdvTemplateDir = this is IAddStandardAdventureMenu ? 
				Constants.AdventuresDir + @"\AdventureTemplates\Standard" : 
				Constants.AdventuresDir + @"\AdventureTemplates\Custom";
		}
	}
}
