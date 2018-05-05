
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, EamonRT.Framework.Commands.IReadyCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Hammer of Thor

			if (DobjArtifact.Uid == 24)
			{
				Globals.Out.Print("Only Thor himself could do that.");

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
