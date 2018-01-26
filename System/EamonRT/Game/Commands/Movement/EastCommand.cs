
// EastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class EastCommand : Command, IEastCommand
	{
		protected override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.East;
			});
		}

		public EastCommand()
		{
			SortOrder = 20;

			IsDarkEnabled = true;

			Name = "EastCommand";

			Verb = "east";

			Type = Enums.CommandType.Movement;
		}
	}
}
