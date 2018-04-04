
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

		protected override long GetMonsterFleeingMemberCount()
		{
			return Globals.DeviceOpened ? ActorMonster.GroupCount : base.GetMonsterFleeingMemberCount();
		}
	}
}
