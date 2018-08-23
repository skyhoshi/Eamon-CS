
// NeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NeCommand : Command, INeCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.Northeast;
			});
		}

		public NeCommand()
		{
			SortOrder = 60;

			IsDarkEnabled = true;

			Name = "NeCommand";

			Verb = "ne";

			Type = Enums.CommandType.Movement;
		}
	}
}

/* EamonCsCodeTemplate

// NeCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class NeCommand : EamonRT.Game.Commands.NeCommand, INeCommand
	{

	}
}
EamonCsCodeTemplate */
