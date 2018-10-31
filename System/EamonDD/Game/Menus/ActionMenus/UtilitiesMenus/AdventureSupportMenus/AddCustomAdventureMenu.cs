
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
		protected virtual string ProgramCsText { get; set; } =
@"
// Program.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

/*

*/

using Eamon.Framework.Portability;

namespace YourAdventureName
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = ""YourAdventureName"";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
";

		protected virtual string[] IPluginCsText { get; set; } = new string[]
		{
@"
// IPluginClassMappings.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginClassMappings : EamonRT.Framework.Plugin.IPluginClassMappings
	{

	}
}
",
@"
// IPluginConstants.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginConstants : EamonRT.Framework.Plugin.IPluginConstants
	{
		
	}
}
",
@"
// IPluginGlobals.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{

	}
}
"
		};

		protected virtual string[] PluginCsText { get; set; } = new string[]
		{
@"
// PluginClassMappings.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using System.Reflection;
using Eamon;

namespace YourAdventureName.Game.Plugin
{
	public class PluginClassMappings : EamonRT.Game.Plugin.PluginClassMappings, Framework.Plugin.IPluginClassMappings
	{
		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}
	}
}
",
@"
// PluginConstants.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{
		
	}
}
",
@"
// PluginContext.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Game.Plugin
{
	public static class PluginContext
	{
		public static Framework.Plugin.IPluginConstants Constants
		{
			get
			{
				return (Framework.Plugin.IPluginConstants)EamonRT.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static Framework.Plugin.IPluginClassMappings ClassMappings
		{
			get
			{
				return (Framework.Plugin.IPluginClassMappings)EamonRT.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static Framework.Plugin.IPluginGlobals Globals
		{
			get
			{
				return (Framework.Plugin.IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}
	}
}
",
@"
// PluginGlobals.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{

	}
}
"
		};

		protected virtual string EngineCsText { get; set; } =
@"
// Engine.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{

	}
}
";

		protected virtual string ChangeLogText { get; set; } =
@"
==================================================================================================================================
ChangeLog: YourAdventureName
==================================================================================================================================

Date			Version			Who			Notes
----------------------------------------------------------------------------------------------------------------------------------
20XXXXXX		1.5.0				YourAuthorInitials			Code complete 1.5.0
";

		protected virtual string AdventureCsprojText { get; set; } =
@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <Version>1.5.0.0</Version>
    <Authors>YourAuthorName</Authors>
    <Company>YourAuthorName</Company>
    <Product>The Wonderful World of Eamon CS</Product>
    <Description>Eamon CS Adventure Plugin</Description>
    <Copyright>Copyright (C) 2014+</Copyright>
  </PropertyGroup>

  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|AnyCPU'"">
    <DefineConstants>TRACE;DEBUG;NETSTANDARD2_0;PORTABLE</DefineConstants>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <OutputPath>..\..\System\Bin\</OutputPath>
    <DocumentationFile>..\..\System\Bin\YourAdventureName.xml</DocumentationFile>
    <NoWarn>0419;1574;1591;1701;1702;1705</NoWarn>
  </PropertyGroup>

  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|AnyCPU'"">
    <DefineConstants>TRACE;RELEASE;NETSTANDARD2_0;PORTABLE</DefineConstants>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <OutputPath>..\..\System\Bin\</OutputPath>
    <DocumentationFile>..\..\System\Bin\YourAdventureName.xml</DocumentationFile>
    <NoWarn>0419;1574;1591;1701;1702;1705</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include=""Eamon"">
      <HintPath>..\..\System\Bin\Eamon.dll</HintPath>
    </Reference>
    <Reference Include=""EamonDD"">
      <HintPath>..\..\System\Bin\EamonDD.dll</HintPath>
    </Reference>
    <Reference Include=""EamonRT"">
      <HintPath>..\..\System\Bin\EamonRT.dll</HintPath>
    </Reference>
  </ItemGroup>

</Project>
";

		protected virtual void CreateCustomFiles()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin");

			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\ChangeLog.txt", ReplaceMacros(ChangeLogText));

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Program.cs", ReplaceMacros(ProgramCsText));

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj", ReplaceMacros(AdventureCsprojText));

			var fileNames = new string[] { "IPluginClassMappings.cs", "IPluginConstants.cs", "IPluginGlobals.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin\" + fileNames[i], ReplaceMacros(IPluginCsText[i]));
			}

			fileNames = new string[] { "PluginClassMappings.cs", "PluginConstants.cs", "PluginContext.cs", "PluginGlobals.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin\" + fileNames[i], ReplaceMacros(PluginCsText[i]));
			}

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Engine.cs", ReplaceMacros(EngineCsText));

			fileNames = new string[] { "Artifact.cs", "Effect.cs", "GameState.cs", "Hint.cs", "Module.cs", "Monster.cs", "Room.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				IncludeInterface = (i == 2);

				ParentClassFileName = @"..\Eamon\Game\" + fileNames[i];

				CreateCustomClassFile();
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

			CreateQuickLaunchFiles();

			CreateAdventureFolder();

			CreateHintsXml();

			CreateCustomFiles();

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
