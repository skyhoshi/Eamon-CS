
// EarnCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.Commands
{
	[ClassMappings]
	public class EarnCommand : EamonRT.Game.Commands.Command, Framework.Commands.IEarnCommand
	{
		protected override void PlayerExecute()
		{
			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public EarnCommand()
		{
			SortOrder = 480;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "EarnCommand";

			Verb = "earn";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
