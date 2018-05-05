
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, EamonRT.Framework.Commands.IFreeCommand
	{
		public FreeCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
