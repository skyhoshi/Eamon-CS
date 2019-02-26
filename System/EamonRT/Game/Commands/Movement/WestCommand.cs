
// WestCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WestCommand : Command, IWestCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.West;
			});
		}

		public WestCommand()
		{
			SortOrder = 30;

			IsDarkEnabled = true;

			Name = "WestCommand";

			Verb = "west";

			Type = CommandType.Movement;
		}
	}
}
