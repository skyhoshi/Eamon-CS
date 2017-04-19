
// RemoveCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using ARuncibleCargo.Framework.Commands;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IRemoveCommand))]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		protected override bool IsDobjArtifactDisguisedMonster()
		{
			// Bill in oven, Lil in cell

			return DobjArtifact.Uid != 82 && DobjArtifact.Uid != 135 && base.IsDobjArtifactDisguisedMonster();
		}
	}
}
