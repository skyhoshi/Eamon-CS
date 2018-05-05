
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, EamonRT.Framework.Commands.IRemoveCommand
	{
		protected override bool IsDobjArtifactDisguisedMonster()
		{
			// Bill in oven, Lil in cell

			return DobjArtifact.Uid != 82 && DobjArtifact.Uid != 135 && base.IsDobjArtifactDisguisedMonster();
		}
	}
}
