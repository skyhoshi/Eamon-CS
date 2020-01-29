
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void PrintDontNeedTo()
		{
			Debug.Assert(gDobjArtifact != null);

			if (gDobjArtifact.Uid == 3 || gDobjArtifact.Uid == 4 || gDobjArtifact.Uid == 5 || gDobjArtifact.Uid == 13 || gDobjArtifact.Uid == 35)
			{
				PrintNotOpen(gDobjArtifact);
			}
			else
			{
				base.PrintDontNeedTo();
			}
		}
	}
}
