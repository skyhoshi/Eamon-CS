
// UpCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class UpCommand : Command, IUpCommand
	{
		protected override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.Up;
			});
		}

		public UpCommand()
		{
			SortOrder = 40;

			IsDarkEnabled = true;

			Name = "UpCommand";

			Verb = "up";

			Type = Enums.CommandType.Movement;
		}
	}
}
