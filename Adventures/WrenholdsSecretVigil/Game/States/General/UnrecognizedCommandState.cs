
// UnrecognizedCommandState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IUnrecognizedCommandState))]
	public class UnrecognizedCommandState : EamonRT.Game.States.UnrecognizedCommandState, IUnrecognizedCommandState
	{
		public override void Execute()
		{
			Globals.Out.Write("{0}Pray thee adventurer, please use these commands ---{0}", Environment.NewLine);

			base.Execute();
		}
	}
}
