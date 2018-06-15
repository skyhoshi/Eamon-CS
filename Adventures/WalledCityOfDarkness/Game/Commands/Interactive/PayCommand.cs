
// PayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.Commands
{
	[ClassMappings]
	public class PayCommand : EamonRT.Game.Commands.Command, Framework.Commands.IPayCommand
	{
		public override void PlayerExecute()
		{
			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{

		}

		public PayCommand()
		{
			SortOrder = 470;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "PayCommand";

			Verb = "pay";

			Type = Enums.CommandType.Interactive;
		}
	}
}
