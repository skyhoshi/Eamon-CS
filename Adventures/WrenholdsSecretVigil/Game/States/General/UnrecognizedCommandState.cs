
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class UnrecognizedCommandState : EamonRT.Game.States.UnrecognizedCommandState, EamonRT.Framework.States.IUnrecognizedCommandState
	{
		public override void Execute()
		{
			Globals.Out.Print("Pray thee adventurer, please use these commands ---");

			base.Execute();
		}
	}
}
