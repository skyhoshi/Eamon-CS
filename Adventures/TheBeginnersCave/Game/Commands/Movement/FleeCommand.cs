
// FleeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IFleeCommand))]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		protected override void PrintCalmDown()
		{
			Globals.Out.WriteLine("{0}What are you fleeing from?", Environment.NewLine);
		}

		protected override void PrintNoPlaceToGo()
		{
			Globals.Out.WriteLine("{0}There's no place to run!", Environment.NewLine);
		}

		protected override void PlayerProcessEvents()
		{
			// another classic Eamon moment...

			var monster = Globals.MDB[7];

			Debug.Assert(monster != null);

			if (monster.IsInRoom(ActorRoom))
			{
				Globals.Out.WriteLine("{0}You are held fast by the mimic and cannot flee!", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

				GotoCleanup = true;
			}

			base.PlayerProcessEvents();
		}
	}
}
