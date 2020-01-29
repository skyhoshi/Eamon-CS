
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterArtifactRead)
			{
				// Scroll vanishes

				if (gDobjArtifact.Uid == 3)
				{
					gDobjArtifact.SetInLimbo();
				}
			}

			base.PlayerProcessEvents(eventType);
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			// Gate at entrance/exit

			if (gDobjArtifact.Uid == 19 || gDobjArtifact.Uid == 20)
			{
				var command = Globals.CreateInstance<IExamineCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
