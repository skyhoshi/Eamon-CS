
// PowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void PlayerProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterPlayerSpellCastCheck)
			{
				gEngine.PrintEffectDesc(45);
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
