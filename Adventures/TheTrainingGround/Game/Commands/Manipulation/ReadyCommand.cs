
// ReadyCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework.Commands;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IReadyCommand))]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Hammer of Thor

			if (DobjArtifact.Uid == 24)
			{
				Globals.Out.WriteLine("{0}Only Thor himself could do that.", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
