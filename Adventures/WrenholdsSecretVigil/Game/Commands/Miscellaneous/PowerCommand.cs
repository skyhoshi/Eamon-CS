
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				Globals.Engine.PrintEffectDesc(45);
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			return false;
		}
	}
}
