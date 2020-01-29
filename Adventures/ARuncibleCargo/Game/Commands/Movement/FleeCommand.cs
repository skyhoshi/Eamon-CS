
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		public override bool IsAllowedInRoom()
		{
			// Disable FleeCommand in water rooms

			return !gActorRoom.IsWaterRoom();
		}
	}
}
