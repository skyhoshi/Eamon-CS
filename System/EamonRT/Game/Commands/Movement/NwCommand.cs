
// NwCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NwCommand : Command, INwCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.Northwest;
			});
		}

		public NwCommand()
		{
			SortOrder = 70;

			IsDarkEnabled = true;

			Name = "NwCommand";

			Verb = "nw";

			Type = Enums.CommandType.Movement;
		}
	}
}

/* EamonCsCodeTemplate

// NwCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class NwCommand : EamonRT.Game.Commands.NwCommand, INwCommand
	{

	}
}
EamonCsCodeTemplate */
