
// StateImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.States;
using Eamon.Game.Attributes;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.States
{
	[ClassMappings]
	public class StateImpl : EamonRT.Game.States.StateImpl, IStateImpl
	{
		public override void PrintEnemiesNearby()
		{
			Globals.Out.Print("You can't turn your back on an enemy!");
		}
	}
}
