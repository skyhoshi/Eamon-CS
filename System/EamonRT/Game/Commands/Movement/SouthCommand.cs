
// SouthCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SouthCommand : Command, ISouthCommand
	{
		protected override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.South;
			});
		}

		public SouthCommand()
		{
			SortOrder = 10;

			IsDarkEnabled = true;

			Name = "SouthCommand";

			Verb = "south";

			Type = Enums.CommandType.Movement;
		}
	}
}
