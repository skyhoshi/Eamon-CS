
// DownCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class DownCommand : Command, IDownCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.Down;
			});
		}

		public DownCommand()
		{
			SortOrder = 50;

			IsDarkEnabled = true;

			Name = "DownCommand";

			Verb = "down";

			Type = Enums.CommandType.Movement;
		}
	}
}

/* EamonCsCodeTemplate

// DownCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class DownCommand : EamonRT.Game.Commands.DownCommand, IDownCommand
	{

	}
}
EamonCsCodeTemplate */
