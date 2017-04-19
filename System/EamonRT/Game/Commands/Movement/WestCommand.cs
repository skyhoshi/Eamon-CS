
// WestCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WestCommand : Command, IWestCommand
	{
		protected override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.West;
			});
		}

		public WestCommand()
		{
			SortOrder = 30;

			IsDarkEnabled = true;

			Name = "WestCommand";

			Verb = "west";

			Type = Enums.CommandType.Movement;
		}
	}
}
