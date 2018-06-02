
// BuyCommand.cs

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
	public class BuyCommand : EamonRT.Game.Commands.Command, Framework.Commands.IBuyCommand
	{
		protected override void PlayerExecute()
		{

		}

		protected override void PlayerFinishParsing()
		{

		}

		public BuyCommand()
		{
			SortOrder = 450;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "BuyCommand";

			Verb = "buy";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
