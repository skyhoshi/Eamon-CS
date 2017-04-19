
// DrinkBeerMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DrinkBeerMenu : Menu, IDrinkBeerMenu
	{
		public override void Execute()
		{
			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}As you go over to the men, you feel a sword being thrust through your back and you hear someone say, \"You really must learn to follow directions!\"{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);
		}

		public DrinkBeerMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
