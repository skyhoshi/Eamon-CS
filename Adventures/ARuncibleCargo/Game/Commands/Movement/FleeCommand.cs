
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, EamonRT.Framework.Commands.IFleeCommand
	{
		protected override bool IsAllowedInRoom()
		{
			// Disable FleeCommand in water rooms

			return !ActorRoom.CastTo<Framework.IRoom>().IsWaterRoom();
		}
	}
}
