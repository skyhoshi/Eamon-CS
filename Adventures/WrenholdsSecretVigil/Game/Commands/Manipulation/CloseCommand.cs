
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, EamonRT.Framework.Commands.ICloseCommand
	{
		protected override void PrintBrokeIt(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				Globals.Out.Print("You mangled {0}!", artifact.EvalPlural("it", "them"));
			}
			else
			{
				base.PrintBrokeIt(artifact);
			}
		}
	}
}
