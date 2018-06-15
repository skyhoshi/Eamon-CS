
// SwCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SwCommand : Command, ISwCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.Southwest;
			});
		}

		public SwCommand()
		{
			SortOrder = 90;

			IsDarkEnabled = true;

			Name = "SwCommand";

			Verb = "sw";

			Type = Enums.CommandType.Movement;
		}
	}
}
