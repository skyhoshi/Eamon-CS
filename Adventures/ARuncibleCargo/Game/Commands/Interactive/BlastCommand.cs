
// BlastCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using ARuncibleCargo.Framework.Commands;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IBlastCommand))]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		protected override bool IsAllowedInRoom()
		{
			// Disable BlastCommand in water rooms

			return !ActorRoom.CastTo<Framework.IRoom>().IsWaterRoom();
		}
	}
}
