
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, EamonRT.Framework.Commands.IFleeCommand
	{
		protected override bool ShouldMonsterFlee()
		{
			return Globals.DeviceOpened || base.ShouldMonsterFlee();
		}

		protected override long GetMonsterFleeingMemberCount()
		{
			return Globals.DeviceOpened ? ActorMonster.GroupCount : base.GetMonsterFleeingMemberCount();
		}
	}
}
