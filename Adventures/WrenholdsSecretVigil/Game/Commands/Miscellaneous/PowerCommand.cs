
// PowerCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPowerCommand))]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
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
