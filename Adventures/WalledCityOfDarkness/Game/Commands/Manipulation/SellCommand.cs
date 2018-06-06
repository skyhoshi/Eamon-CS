
// SellCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.Commands
{
	[ClassMappings]
	public class SellCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISellCommand
	{
		protected override void PlayerExecute()
		{
			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{

		}

		public SellCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "SellCommand";

			Verb = "sell";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
