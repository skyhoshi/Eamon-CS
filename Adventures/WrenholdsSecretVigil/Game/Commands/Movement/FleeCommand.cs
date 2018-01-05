
// FleeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IFleeCommand))]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		protected override bool ShouldMonsterFlee()
		{
			return Globals.DeviceOpened || base.ShouldMonsterFlee();
		}
	}
}
