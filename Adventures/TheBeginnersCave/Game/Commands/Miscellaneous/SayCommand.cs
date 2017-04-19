
// SayCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ISayCommand))]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		protected override void PlayerProcessEvents01()
		{
			if (ProcessedPhrase.IndexOf("trollsfire", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				var command = Globals.CreateInstance<ITrollsfireCommand>();

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
