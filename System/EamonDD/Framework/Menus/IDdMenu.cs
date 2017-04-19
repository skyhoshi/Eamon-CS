
// IDdMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace EamonDD.Framework.Menus
{
	public interface IDdMenu
	{
		void PrintMainMenuSubtitle();

		void PrintConfigMenuSubtitle();

		void PrintFilesetMenuSubtitle();

		void PrintCharacterMenuSubtitle();

		void PrintModuleMenuSubtitle();

		void PrintRoomMenuSubtitle();

		void PrintArtifactMenuSubtitle();

		void PrintEffectMenuSubtitle();

		void PrintMonsterMenuSubtitle();

		void PrintHintMenuSubtitle();
	};
}
