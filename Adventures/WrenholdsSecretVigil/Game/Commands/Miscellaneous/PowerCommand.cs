
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, EamonRT.Framework.Commands.IPowerCommand
	{
		protected override void PlayerProcessEvents()
		{
			Globals.Engine.PrintEffectDesc(45);
		}

		protected override bool ShouldAllowSkillGains()
		{
			return false;
		}
	}
}
