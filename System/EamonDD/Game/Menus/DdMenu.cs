
// DdMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus
{
	[ClassMappings]
	public class DdMenu : IDdMenu
	{
		public virtual void PrintMainMenuSubtitle()
		{
			long i;

			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Write("{0}Configs: 1", Environment.NewLine);

			if (Globals.Config.DdEditingFilesets)
			{
				Globals.Out.Write("{0}Filesets: {1}", "  ", Globals.Database.GetFilesetsCount());
			}

			if (Globals.Config.DdEditingCharacters)
			{
				Globals.Out.Write("{0}Characters: {1}", "  ", Globals.Database.GetCharactersCount());
			}

			if (Globals.Config.DdEditingModules)
			{
				Globals.Out.Write("{0}Modules: {1}", "  ", Globals.Database.GetModulesCount());
			}

			if (Globals.Config.DdEditingRooms)
			{
				Globals.Out.Write("{0}Rooms: {1}", "  ", Globals.Database.GetRoomsCount());
			}

			Globals.Out.WriteLine();

			i = 0;

			if (Globals.Config.DdEditingArtifacts || Globals.Config.DdEditingEffects || Globals.Config.DdEditingMonsters || Globals.Config.DdEditingHints)
			{
				Globals.Out.WriteLine();

				if (Globals.Config.DdEditingArtifacts)
				{
					Globals.Out.Write("Artifacts: {0}", Globals.Database.GetArtifactsCount());

					i++;
				}

				if (Globals.Config.DdEditingEffects)
				{
					Globals.Out.Write("{0}Effects: {1}", i > 0 ? "  " : "", Globals.Database.GetEffectsCount());

					i++;
				}

				if (Globals.Config.DdEditingMonsters)
				{
					Globals.Out.Write("{0}Monsters: {1}", i > 0 ? "  " : "", Globals.Database.GetMonstersCount());

					i++;
				}

				if (Globals.Config.DdEditingHints)
				{
					Globals.Out.Write("{0}Hints: {1}", i > 0 ? "  " : "", Globals.Database.GetHintsCount());

					i++;
				}

				Globals.Out.WriteLine();
			}
		}

		public virtual void PrintConfigMenuSubtitle()
		{
			Globals.Out.Print("Configs: 1");
		}

		public virtual void PrintFilesetMenuSubtitle()
		{
			Globals.Out.Print("Filesets: {0}", Globals.Database.GetFilesetsCount());
		}

		public virtual void PrintCharacterMenuSubtitle()
		{
			Globals.Out.Print("Characters: {0}", Globals.Database.GetCharactersCount());
		}

		public virtual void PrintModuleMenuSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Print("Modules: {0}", Globals.Database.GetModulesCount());
		}

		public virtual void PrintRoomMenuSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Print("Rooms: {0}", Globals.Database.GetRoomsCount());
		}

		public virtual void PrintArtifactMenuSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Print("Artifacts: {0}", Globals.Database.GetArtifactsCount());
		}

		public virtual void PrintEffectMenuSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Print("Effects: {0}", Globals.Database.GetEffectsCount());
		}

		public virtual void PrintMonsterMenuSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Print("Monsters: {0}", Globals.Database.GetMonstersCount());
		}

		public virtual void PrintHintMenuSubtitle()
		{
			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);
			}

			Globals.Out.Print("Hints: {0}", Globals.Database.GetHintsCount());
		}
	}
}
