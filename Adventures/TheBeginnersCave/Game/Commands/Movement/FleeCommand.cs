
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		protected override void PrintCalmDown()
		{
			Globals.Out.Print("What are you fleeing from?");
		}

		protected override void PrintNoPlaceToGo()
		{
			Globals.Out.Print("There's no place to run!");
		}

		protected override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterNumberOfExitsCheck)
			{
				// another classic Eamon moment...

				var mimicMonster = Globals.MDB[7];

				Debug.Assert(mimicMonster != null);

				if (mimicMonster.IsInRoom(ActorRoom))
				{
					Globals.Out.Print("You are held fast by the mimic and cannot flee!");

					NextState = Globals.CreateInstance<IMonsterStartState>();

					GotoCleanup = true;
				}
			}

			base.PlayerProcessEvents(eventType);
		}
	}
}
