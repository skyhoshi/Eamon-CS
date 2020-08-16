
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(EventType eventType)
		{
			//          Spook Reducer 2.0
			//  (c) 2012 Frank Black Productions

			if (eventType == EventType.AfterPlayerSay && string.Equals(ProcessedPhrase, "less spooks", StringComparison.OrdinalIgnoreCase) && gGameState.SpookCounter < 8)
			{
				var spookMonster = gMDB[9];

				Debug.Assert(spookMonster != null);

				spookMonster.GroupCount = spookMonster.GroupCount > 0 ? 1 : 0;

				spookMonster.InitGroupCount = spookMonster.GroupCount;

				spookMonster.OrigGroupCount = spookMonster.GroupCount;

				gGameState.SpookCounter = 8;

				gOut.Print("Less spooks it is!");

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			base.PlayerProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
