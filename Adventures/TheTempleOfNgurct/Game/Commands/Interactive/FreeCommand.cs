
// FreeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Commands;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IFreeCommand))]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public FreeCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
