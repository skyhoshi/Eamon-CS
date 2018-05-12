
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public FreeCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
