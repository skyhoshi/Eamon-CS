
// MhMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using EamonMH.Framework.Menus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus
{
	[ClassMappings]
	public class MhMenu : IMhMenu
	{
		public virtual void PrintMainHallMenuSubtitle()
		{
			Globals.Out.WriteLine("{0}Character: {1}", Environment.NewLine, Globals.Character != null ? Globals.Character.Name : Globals.Engine.UnknownName);

			Globals.Out.WriteLine("{0}As you wander about the hall, you realize you can do one of seven things:", Environment.NewLine);
		}

		public virtual void PrintVillageMenuSubtitle()
		{
			Globals.Out.WriteLine("{0}Character: {1}", Environment.NewLine, Globals.Character != null ? Globals.Character.Name : Globals.Engine.UnknownName);

			Globals.Out.WriteLine("{0}As you wander about the village, you realize you can do one of seven things:", Environment.NewLine);
		}

		public virtual void PrintPracticeAreaMenuSubtitle()
		{
			Globals.Out.WriteLine("{0}Character: {1}", Environment.NewLine, Globals.Character != null ? Globals.Character.Name : Globals.Engine.UnknownName);

			Globals.Out.WriteLine("{0}As you wander about the alleys, you realize you can do one of six things:", Environment.NewLine);
		}
	}
}
