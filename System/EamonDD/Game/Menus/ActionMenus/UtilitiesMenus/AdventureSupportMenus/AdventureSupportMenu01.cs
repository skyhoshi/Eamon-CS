
// AdventureSupportMenu01.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Automation;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using DDEnums = EamonDD.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AdventureSupportMenu01 : Menu, IAdventureSupportMenu01
	{
		protected virtual bool GotoCleanup { get; set; }

		protected virtual bool IncludeInterface { get; set; }

		protected virtual DDEnums.SupportMenuType SupportMenuType { get; set; }

		protected virtual string AdventureName { get; set; }

		protected virtual string AdventureName01 { get; set; }

		protected virtual string AuthorName { get; set; }

		protected virtual string AuthorInitials { get; set; }

		protected virtual string ParentClassFileName { get; set; }

		protected virtual string HintsXmlText { get; set; } =
@"<Complex name=""Root"" type=""Eamon.Game.DataStorage.HintDbTable, Eamon, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null"">
  <Properties>
    <Collection name=""Records"" type=""Eamon.ThirdParty.BTree`1[[Eamon.Framework.IHint, Eamon, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null]], Eamon, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null"">
      <Properties>
        <Simple name=""IsReadOnly"" value=""False"" />
        <Simple name=""AllowDuplicates"" value=""False"" />
      </Properties>
      <Items>
        <Complex type=""YourAdventureName.Game.Hint, YourAdventureName, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null"">
          <Properties>
            <Simple name=""Uid"" value=""1"" />
            <Simple name=""IsUidRecycled"" value=""True"" />
            <Simple name=""Active"" value=""True"" />
            <Simple name=""Question"" value=""EAMON CS 1.4 GENERAL HELP."" />
            <Simple name=""NumAnswers"" value=""8"" />
            <SingleArray name=""Answers"">
              <Items>
                <Simple value=""1. Commands may be abbreviated on the left or right side.  Examples:  &quot;A DR&quot; or &quot;A GON&quot; for ATTACK DRAGON, &quot;REQ RAY FROM AL&quot; or &quot;REQ GUN FROM IEN&quot; for REQUEST RAY GUN FROM ALIEN, &quot;PUT PUM IN LAR&quot; or &quot;PUT PIE IN OVEN&quot; for PUT PUMPKIN PIE IN **999"" />
                <Simple value=""2. Sometimes items may be in a room but won't show up until you EXAMINE them.  Pay close attention to all descriptions.  Note:  LOOK only repeats the room description and -nothing- else.  Use EXAMINE to, well, examine things."" />
                <Simple value=""3. Before you can manipulate items that are inside of other items, you must REMOVE them from that item.  There is no REMOVE ALL command, but if you ATTACK a (breakable) container, all of its contents will be emptied to the floor and you can GET ALL."" />
                <Simple value=""4. Type SAVE and a number for a desired save position to Quick Save (save without having to verify).  Adding a description will rename that slot.  Examples:  &quot;SAVE 2&quot; or &quot;SAVE 5 Dark Room (NO GRUES)&quot;.  Also works with RESTORE.  (Type RESTORE 1, etc.)"" />
                <Simple value=""5. You can INVENTORY companions (normally anyone whom, when you SMILE, smiles back at you) to see what they are carrying and wearing.  You can then REQUEST those items from them.  You can also INVENTORY containers to get a list of contents."" />
                <Simple value=""6. If you GIVE food or a beverage to a friend, they will take a bite or drink and give it back to you."" />
                <Simple value=""7. To give money to someone, type GIVE and an amount.  For example, GIVE 1000 TO IRS AGENT would pay 1000 gold coins to the nice IRS Agent.  In most standard adventures, if you GIVE 5000 or more, a neutral monster will -usually- become your friend."" />
                <Simple value=""8. The POWER spell has been known to have strange and marvelous effects in many adventures.  When all else fails (or just for fun) try POWER."" />
              </Items>
            </SingleArray>
          </Properties>
        </Complex>
        <Complex type=""YourAdventureName.Game.Hint, YourAdventureName, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null"">
          <Properties>
            <Simple name=""Uid"" value=""2"" />
            <Simple name=""IsUidRecycled"" value=""True"" />
            <Simple name=""Active"" value=""True"" />
            <Simple name=""Question"" value=""EAMON CS 1.4 NEW COMMANDS."" />
            <Simple name=""NumAnswers"" value=""1"" />
            <SingleArray name=""Answers"">
              <Items>
                <Simple value=""1. The GO command allows you to travel through any door/gate you encounter, including those that are free-standing (not associated with a room directional link)."" />
              </Items>
            </SingleArray>
          </Properties>
        </Complex>
      </Items>
    </Collection>
    <Collection name=""FreeUids"" type=""System.Collections.Generic.List`1[[System.Int64, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
      <Properties>
        <Simple name=""Capacity"" value=""0"" />
      </Properties>
      <Items />
    </Collection>
    <Simple name=""CurrUid"" value=""2"" />
  </Properties>
</Complex>";

		protected virtual string EditAdventureShText { get; set; }

		protected virtual string ResumeAdventureShText { get; set; }

		protected virtual string EditAdventureBatText { get; set; } =
@"@echo off
cd ..\..\..\System\Bin
dotnet .\EamonPM.WindowsUnix.dll -pfn YourLibraryName.dll -wd ..\..\Adventures\YourAdventureName -la -rge
";

		protected virtual string ResumeAdventureBatText { get; set; } =
@"@echo off
cd ..\..\..\System\Bin
dotnet .\EamonPM.WindowsUnix.dll -pfn YourLibraryName.dll -wd ..\..\Adventures\YourAdventureName
";

		protected virtual string InterfaceCsText { get; set; } =
@"
// YourInterfaceName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourFrameworkNamespaceName
{
	public interface YourInterfaceName : EamonLibraryName.YourFrameworkNamespaceName.YourInterfaceName
	{

	}
}";

		protected virtual string ClassWithInterfaceCsText { get; set; } =
@"
// YourClassName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

YourEamonUsingStatementusing Eamon.Game.Attributes;
YourEamonRTUsingStatementusing static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourGameNamespaceName
{
	[ClassMappings(typeof(YourInterfaceName))]
	public class YourClassName : EamonLibraryName.YourGameNamespaceName.YourClassName, YourFrameworkNamespaceName.YourInterfaceName
	{

	}
}
";

		protected virtual string ClassCsText { get; set; } =
@"
// YourClassName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

YourEamonUsingStatementusing Eamon.Game.Attributes;
YourEamonRTUsingStatementusing static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourGameNamespaceName
{
	[ClassMappings]
	public class YourClassName : EamonLibraryName.YourGameNamespaceName.YourClassName, YourInterfaceName
	{

	}
}
";

		protected virtual IList<string> SelectedAdvDbTextFiles { get; set; }

		protected virtual IList<string> SelectedClassFiles { get; set; }

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
				try
				{
					VsaAssembly = Assembly.LoadFrom(Globals.Path.GetFullPath(@".\EamonVS.dll"));
				}
				catch (Exception)
				{
					VsaAssembly = null;
				}
			}
		}

		protected virtual void GetVsaObjectIfNecessary()
		{
			if (VsaAssembly != null && VsaObject == null)
			{
				Type type = null;

				try
				{
					type = VsaAssembly.GetType("EamonVS.VisualStudioAutomation");
				}
				catch (Exception)
				{
					type = null;
				}

				if (type != null)
				{
					try
					{
						VsaObject = (IVisualStudioAutomation)Activator.CreateInstance(type);
					}
					catch (Exception)
					{
						VsaObject = null;
					}

					if (VsaObject != null)
					{
						VsaObject.DevenvExePath = Globals.DevenvExePath;

						VsaObject.SolutionFile = Globals.Path.GetFullPath(Constants.EamonAdventuresSlnFile);
					}
				}
			}
		}

		protected virtual void CheckForPrerequisites()
		{
			if (!Globals.File.Exists(Globals.DevenvExePath))
			{
				if (SupportMenuType == DDEnums.SupportMenuType.DeleteAdventure)
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
				if (SupportMenuType == DDEnums.SupportMenuType.DeleteAdventure)
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
				if (SupportMenuType == DDEnums.SupportMenuType.DeleteAdventure)
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
			else if (!Globals.File.Exists(Globals.Path.GetFullPath(Constants.EamonAdventuresSlnFile)))
			{
				if (SupportMenuType == DDEnums.SupportMenuType.DeleteAdventure)
				{
					Globals.Out.Print("{0}", Globals.LineSep);
				}

				Globals.Out.Print("Could not locate the Eamon.Adventures solution at the following path:");

				Globals.Out.WordWrap = false;

				Globals.Out.Print(Globals.Path.GetFullPath(Constants.EamonAdventuresSlnFile));

				Globals.Out.WordWrap = true;

				Globals.Out.Print(@"This Eamon CS repository may be compromised since the solution should always be present.");

				GotoCleanup = true;
			}

			if (GotoCleanup)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not {0}.", 
					SupportMenuType == DDEnums.SupportMenuType.AddAdventure ? "created" :
					SupportMenuType == DDEnums.SupportMenuType.DeleteAdventure ? "deleted" :
					"processed");
			}
		}

		protected virtual void GetAdventureName()
		{
			var invalidAdventureNames = new string[] { "Adventures", "Catalog", "Characters", "Contemporary", "Fantasy", "SciFi", "Test", "Workbench", "WorkInProgress", "AdventureSupportMenu", "AdventureTemplates", "YourAdventureName", "YourAuthorName", "YourAuthorInitials" };

			if (SupportMenuType == DDEnums.SupportMenuType.AddAdventure)
			{
				Globals.Out.Print("You must enter a name for your new adventure (eg, The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures; input should always be properly title-cased.");

				Globals.Out.Print("Note:  the name will be used to produce a shortened form suitable for use as a folder name under the Adventures directory and also as a C# namespace (eg, TheBeginnersCave).");
			}
			else
			{
				if (SupportMenuType == DDEnums.SupportMenuType.AddClasses)
				{
					Globals.Out.Print(@"Note:  this menu option will allow you to enter the file paths for interfaces or classes you wish to add to the adventure; the actual addition will occur after you are given a final warning.  Your working directory is System and you should enter relative file paths (eg, .\Eamon\Game\Monster.cs or .\EamonRT\Game\Combat\CombatSystem.cs).  For any classes added, the corresponding .XML textfiles (if any) will be updated appropriately.");
				}
				else if (SupportMenuType == DDEnums.SupportMenuType.DeleteClasses)
				{
					Globals.Out.Print(@"Note:  this menu option will allow you to enter the file paths for interfaces or classes you wish to remove from the adventure; the actual deletion will occur after you are given a final warning.  Your working directory is the adventure folder for the game you've selected and you should enter relative file paths (eg, .\Game\Monster.cs).  For any classes deleted, the corresponding .XML textfiles (if any) will be updated appropriately.");
				}

				Globals.Out.Print("You must enter the name of the adventure you wish to {0} (eg, The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures; input should always be properly title-cased.", SupportMenuType == DDEnums.SupportMenuType.DeleteAdventure ? "delete" : "process");
			}

			AdventureName = string.Empty;

			while (AdventureName.Length == 0)
			{
				Globals.Out.Write("{0}Enter the name of the {1}adventure: ", Environment.NewLine, SupportMenuType == DDEnums.SupportMenuType.AddAdventure ? "new " : "");

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, Globals.Engine.IsCharAnyButBackForwardSlash, null);

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

			if (SupportMenuType == DDEnums.SupportMenuType.AddAdventure && Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure already exists.");

				GotoCleanup = true;
			}
			else if (SupportMenuType != DDEnums.SupportMenuType.AddAdventure && !Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure does not exist.");

				GotoCleanup = true;
			}
			else if (SupportMenuType == DDEnums.SupportMenuType.AddClasses || SupportMenuType == DDEnums.SupportMenuType.DeleteClasses)
			{
				if (!Globals.File.Exists(Constants.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj"))
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Print("That is not a custom adventure.");

					GotoCleanup = true;
				}
				else if (!Globals.File.Exists(@".\" + AdventureName + ".dll"))
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Print("The custom adventure library (.dll) does not exist.");

					GotoCleanup = true;
				}
				else if (Globals.File.Exists(Constants.AdventuresDir + @"\" + AdventureName + @"\FRESHMEAT.XML"))
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Print("The adventure is being played through.");

					GotoCleanup = true;
				}
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

			if (SupportMenuType == DDEnums.SupportMenuType.AddAdventure)
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

						if (!string.Equals(advDbTextFile, "ADVENTURES.XML"))
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
					Globals.Out.Print("If you would like to {0} one or more custom adventure databases, enter those file names now (eg, HORROR.XML).  To skip this step, or if you are done, just press enter.", SupportMenuType == DDEnums.SupportMenuType.AddAdventure ? "add this adventure to" : "delete this adventure from");
				}

				Globals.Out.Write("{0}Enter name of custom adventure database: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharAlnumPeriodUnderscore, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				customAdvDbTextFile = Buf.Trim().ToString();

				if (customAdvDbTextFile.Length > 0)
				{
					if (SelectedAdvDbTextFiles.FirstOrDefault(fn => string.Equals(fn, customAdvDbTextFile, StringComparison.OrdinalIgnoreCase)) == null)
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

		protected virtual void QueryToProcessAdventure()
		{
			RetCode rc;

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("WARNING:  you are about to {0} the following classes and update any associated .XML files.  If you have any doubts, you should select 'N' and backup your Eamon CS repository before proceeding.  This action is PERMANENT!", SupportMenuType == DDEnums.SupportMenuType.DeleteClasses ? "delete" : "add");

			foreach (var selectedClassFile in SelectedClassFiles)
			{
				Globals.Out.Write("{0}{1}", Environment.NewLine, selectedClassFile);
			}

			Globals.Out.WriteLine();

			Globals.Out.Write("{0}Would you like to {1} these classes {2} the adventure (Y/N): ", 
				Environment.NewLine, 
				SupportMenuType == DDEnums.SupportMenuType.DeleteClasses ? "delete" : "add",
				SupportMenuType == DDEnums.SupportMenuType.DeleteClasses ? "from" : "to");

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("The adventure was not processed.");

				GotoCleanup = true;
			}
		}

		protected virtual void CreateQuickLaunchFiles()
		{
			var yourLibraryName = this is IAddCustomAdventureMenu ? AdventureName : "EamonRT";

			// Note: QuickLaunch files missing in Eamon CS Mobile

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Unix\EamonDD"))
			{
				var fileText = EditAdventureShText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Unix\EamonDD\Edit" + AdventureName + ".sh", ReplaceMacros(fileText));
			}

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Unix\EamonRT"))
			{
				var fileText = ResumeAdventureShText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Unix\EamonRT\Resume" + AdventureName + ".sh", ReplaceMacros(fileText));
			}

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Windows\EamonDD"))
			{
				var fileText = EditAdventureBatText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Windows\EamonDD\Edit" + AdventureName + ".bat", ReplaceMacros(fileText));
			}

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Windows\EamonRT"))
			{
				var fileText = ResumeAdventureBatText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Windows\EamonRT\Resume" + AdventureName + ".bat", ReplaceMacros(fileText));
			}
		}

		protected virtual void CreateAdventureFolder()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName);
		}

		protected virtual void CreateCustomClassFile()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ParentClassFileName));

			var eamonLibraryName = ParentClassFileName.StartsWith(@"..\Eamon\") ? "Eamon" : ParentClassFileName.StartsWith(@"..\EamonDD\") ? "EamonDD" : "EamonRT";

			if (ParentClassFileName.Contains(@"\Game\"))
			{
				var yourClassName = Globals.Path.GetFileNameWithoutExtension(ParentClassFileName);

				var yourInterfaceName = "I" + yourClassName;

				var fileText = Globals.File.ReadAllText(ParentClassFileName);

				var matches = Regex.Matches(fileText, @".*namespace (.+[^ {\n\r\t])");

				if (matches.Count == 1 && matches[0].Groups.Count == 2)
				{
					var yourGameNamespaceName = matches[0].Groups[1].Value + ".";

					var yourFrameworkNamespaceName = yourGameNamespaceName.Replace(".Game.", ".Framework.");

					yourGameNamespaceName = yourGameNamespaceName.Replace(eamonLibraryName + ".", "").TrimEnd('.');

					yourFrameworkNamespaceName = yourFrameworkNamespaceName.Replace(eamonLibraryName + ".", "").TrimEnd('.');

					var childClassFileName = ParentClassFileName.Replace(@"..\" + eamonLibraryName, Constants.AdventuresDir + @"\" + AdventureName);

					var childClassPath = Globals.Path.GetDirectoryName(childClassFileName);

					Globals.Directory.CreateDirectory(childClassPath);

					if (IncludeInterface)
					{
						var childInterfaceFileName = childClassFileName.Replace(@"\Game\", @"\Framework\").Replace(@"\" + yourClassName + ".cs", @"\" + yourInterfaceName + ".cs");

						var childInterfacePath = Globals.Path.GetDirectoryName(childInterfaceFileName);

						Globals.Directory.CreateDirectory(childInterfacePath);

						fileText = InterfaceCsText.Replace("EamonLibraryName", eamonLibraryName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourInterfaceName", yourInterfaceName);

						Globals.File.WriteAllText(childInterfaceFileName, ReplaceMacros(fileText));
					}

					var yourEamonUsingStatement = string.Empty;

					var yourEamonRTUsingStatement = string.Empty;

					if (string.Equals(eamonLibraryName, "Eamon"))
					{
						yourEamonUsingStatement = string.Format("using {0}.{1};{2}", eamonLibraryName, yourFrameworkNamespaceName, Environment.NewLine);
					}
					else
					{
						yourEamonRTUsingStatement = string.Format("using {0}.{1};{2}", eamonLibraryName, yourFrameworkNamespaceName, Environment.NewLine);
					}

					fileText = IncludeInterface ? ClassWithInterfaceCsText : ClassCsText;

					fileText = fileText.Replace("EamonLibraryName", eamonLibraryName).Replace("YourEamonUsingStatement", yourEamonUsingStatement).Replace("YourEamonRTUsingStatement", yourEamonRTUsingStatement).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourGameNamespaceName", yourGameNamespaceName).Replace("YourInterfaceName", yourInterfaceName).Replace("YourClassName", yourClassName);

					Globals.File.WriteAllText(childClassFileName, ReplaceMacros(fileText));
				}
			}
			else
			{
				var yourInterfaceName = Globals.Path.GetFileNameWithoutExtension(ParentClassFileName);

				var fileText = Globals.File.ReadAllText(ParentClassFileName);

				var matches = Regex.Matches(fileText, @".*namespace (.+[^ {\n\r\t])");

				if (matches.Count == 1 && matches[0].Groups.Count == 2)
				{
					var yourFrameworkNamespaceName = matches[0].Groups[1].Value + ".";

					yourFrameworkNamespaceName = yourFrameworkNamespaceName.Replace(eamonLibraryName + ".", "").TrimEnd('.');

					var childInterfaceFileName = ParentClassFileName.Replace(@"..\" + eamonLibraryName, Constants.AdventuresDir + @"\" + AdventureName);

					var childInterfacePath = Globals.Path.GetDirectoryName(childInterfaceFileName);

					Globals.Directory.CreateDirectory(childInterfacePath);

					fileText = InterfaceCsText.Replace("EamonLibraryName", eamonLibraryName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourInterfaceName", yourInterfaceName);

					Globals.File.WriteAllText(childInterfaceFileName, ReplaceMacros(fileText));
				}
			}
		}

		protected virtual void CreateHintsXml()
		{
			var yourAdventureName = this is IAddCustomAdventureMenu ? AdventureName : "Eamon";

			var fileText = HintsXmlText.Replace("YourAdventureName", yourAdventureName);
				
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

				if (SupportMenuType == DDEnums.SupportMenuType.AddAdventure)
				{
					var fileset = Globals.CreateInstance<IFileset>(x =>
					{
						x.Uid = Globals.Database.GetFilesetUid();

						x.IsUidRecycled = true;

						x.Name = AdventureName01;

						x.WorkDir = Constants.AdventuresDir + @"\" + AdventureName;

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
					var fileset = Globals.Database.FilesetTable.Records.FirstOrDefault(fs => string.Equals(fs.WorkDir, Constants.AdventuresDir + @"\" + AdventureName, StringComparison.OrdinalIgnoreCase));

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

		protected virtual void UpdateXmlFileClasses()
		{
			foreach (var selectedClassFile in SelectedClassFiles)
			{
				var className = Globals.Path.GetFileNameWithoutExtension(selectedClassFile);

				var fileName = @".\" + className.ToUpper() + (string.Equals(className, "Module") ? ".XML" : "S.XML");

				if (Globals.File.Exists(fileName))
				{
					if (SupportMenuType == DDEnums.SupportMenuType.AddClasses)
					{
						var fileText = Globals.File.ReadAllText(fileName);

						Globals.File.WriteAllText(fileName, fileText.Replace("Eamon.Game." + className + ", Eamon", AdventureName + ".Game." + className + ", " + AdventureName));
					}
					else if (SupportMenuType == DDEnums.SupportMenuType.DeleteClasses)
					{
						var fileText = Globals.File.ReadAllText(fileName);

						Globals.File.WriteAllText(fileName, fileText.Replace(AdventureName + ".Game." + className + ", " + AdventureName, "Eamon.Game." + className + ", Eamon"));
					}
				}
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

				Globals.Out.Print("The adventure was not {0}.", SupportMenuType == DDEnums.SupportMenuType.AddAdventure ? "created" : "processed");

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

			srcFileName = @".\" + AdventureName + ".xml";

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

		protected virtual void PrintAdventureProcessed()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("The adventure was successfully processed.");
		}

		public AdventureSupportMenu01()
		{
			Buf = Globals.Buf;

			SupportMenuType = this is IAddStandardAdventureMenu || this is IAddCustomAdventureMenu ? DDEnums.SupportMenuType.AddAdventure :
									this is IAddCustomAdventureClassesMenu ? DDEnums.SupportMenuType.AddClasses :
									this is IDeleteAdventureMenu ? DDEnums.SupportMenuType.DeleteAdventure :
									DDEnums.SupportMenuType.DeleteClasses;

			EditAdventureShText = string.Format(@"#!/bin/sh{0}cd ../../../System/Bin{0}xterm -e dotnet ./EamonPM.WindowsUnix.dll -pfn YourLibraryName.dll -wd ../../Adventures/YourAdventureName -la -rge{0}", '\n');

			ResumeAdventureShText = string.Format(@"#!/bin/sh{0}cd ../../../System/Bin{0}xterm -e dotnet ./EamonPM.WindowsUnix.dll -pfn YourLibraryName.dll -wd ../../Adventures/YourAdventureName{0}", '\n');
		}
	}
}
