
// BlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, EamonRT.Framework.Commands.IBlastCommand
	{
		protected override bool IsAllowedInRoom()
		{
			// Disable BlastCommand in water rooms

			return !ActorRoom.CastTo<Framework.IRoom>().IsWaterRoom();
		}
	}
}
