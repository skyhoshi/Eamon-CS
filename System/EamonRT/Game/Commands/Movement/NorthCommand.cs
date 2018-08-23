
// NorthCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NorthCommand : Command, INorthCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.North;
			});
		}

		public NorthCommand()
		{
			SortOrder = 0;

			IsDarkEnabled = true;

			Name = "NorthCommand";

			Verb = "north";

			Type = Enums.CommandType.Movement;
		}
	}
}

/* EamonCsCodeTemplate

// NorthCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class NorthCommand : EamonRT.Game.Commands.NorthCommand, INorthCommand
	{

	}
}
EamonCsCodeTemplate */
