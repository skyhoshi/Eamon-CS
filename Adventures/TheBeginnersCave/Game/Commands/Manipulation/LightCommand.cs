
// LightCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class LightCommand : EamonRT.Game.Commands.LightCommand, ILightCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			if (gDobjArtifact.Uid == 10)
			{
				var command = Globals.CreateInstance<Framework.Commands.ITrollsfireCommand>();

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
