
// FleeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using ARuncibleCargo.Framework.Commands;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IFleeCommand))]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		protected override bool IsAllowedInRoom()
		{
			// Disable FleeCommand in water rooms

			return !ActorRoom.CastTo<Framework.IRoom>().IsWaterRoom();
		}
	}
}
