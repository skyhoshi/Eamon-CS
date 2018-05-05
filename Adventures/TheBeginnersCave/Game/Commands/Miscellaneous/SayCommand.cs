
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, EamonRT.Framework.Commands.ISayCommand
	{
		protected override void PlayerProcessEvents01()
		{
			if (ProcessedPhrase.IndexOf("trollsfire", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				var command = Globals.CreateInstance<Framework.Commands.ITrollsfireCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				base.PlayerProcessEvents01();
			}
		}
	}
}
