
// IPluginGlobals.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus;

namespace EamonDD.Framework.Plugin
{
	public interface IPluginGlobals : Eamon.Framework.Plugin.IPluginGlobals
	{
		string[] Argv { get; set; }

		StringBuilder Buf { get; set; }

		long WordWrapCurrColumn { get; set; }

		char WordWrapLastChar { get; set; }

		string ConfigFileName { get; set; }

		new IEngine Engine { get; set; }

		IDdMenu DdMenu { get; set; }

		IMenu Menu { get; set; }

		IModule Module { get; set; }

		IConfig Config { get; set; }

		bool ConfigsModified { get; set; }

		bool FilesetsModified { get; set; }

		bool CharactersModified { get; set; }

		bool ModulesModified { get; set; }

		bool RoomsModified { get; set; }

		bool ArtifactsModified { get; set; }

		bool EffectsModified { get; set; }

		bool MonstersModified { get; set; }

		bool HintsModified { get; set; }
	}
}
