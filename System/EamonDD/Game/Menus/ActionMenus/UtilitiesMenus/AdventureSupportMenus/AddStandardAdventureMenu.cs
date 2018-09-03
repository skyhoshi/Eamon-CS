
// AddStandardAdventureMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddStandardAdventureMenu : AdventureSupportMenu01, IAddStandardAdventureMenu
	{
		public override void Execute()
		{
			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("ADD STANDARD ADVENTURE", true);

			Debug.Assert(!Globals.Engine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

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

			UpdateAdvDbTextFiles();

			PrintAdventureCreated();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback adventure buildout if necessary
			}
		}

		public AddStandardAdventureMenu()
		{

		}
	}
}
