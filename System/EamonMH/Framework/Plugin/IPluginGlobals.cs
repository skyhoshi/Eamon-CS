
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonMH.Framework.Menus;

namespace EamonMH.Framework.Plugin
{
	public interface IPluginGlobals : Eamon.Framework.Plugin.IPluginGlobals
	{
		string[] Argv { get; set; }

		StringBuilder Buf { get; set; }

		long WordWrapCurrColumn { get; set; }

		char WordWrapLastChar { get; set; }

		string ConfigFileName { get; set; }

		string CharacterName { get; set; }

		new IEngine Engine { get; set; }

		IMhMenu MhMenu { get; set; }

		IMenu Menu { get; set; }

		IFileset Fileset { get; set; }

		ICharacter Character { get; set; }

		IConfig Config { get; set; }

		bool GoOnAdventure { get; set; }

		bool ConfigsModified { get; set; }

		bool FilesetsModified { get; set; }

		bool CharactersModified { get; set; }

		bool EffectsModified { get; set; }
	}
}
