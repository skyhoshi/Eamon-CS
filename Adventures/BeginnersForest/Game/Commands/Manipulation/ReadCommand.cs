
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using BeginnersForest.Framework.Commands;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IReadCommand))]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		protected override void PlayerProcessEvents01()
		{
			// Scroll vanishes

			if (DobjArtifact.Uid == 3)
			{
				DobjArtifact.SetInLimbo();
			}

			base.PlayerProcessEvents01();
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Gate at entrance/exit

			if (DobjArtifact.Uid == 19 || DobjArtifact.Uid == 20)
			{
				var command = Globals.CreateInstance<EamonRT.Framework.Commands.IExamineCommand>();

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
