
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.States
{
	[ClassMappings]
	public class UnrecognizedCommandState : EamonRT.Game.States.UnrecognizedCommandState, IUnrecognizedCommandState
	{
		public override void Execute()
		{
			Globals.Out.Print("These are the valid commands:");

			base.Execute();
		}
	}
}
