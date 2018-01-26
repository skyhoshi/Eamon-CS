
// DropCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Commands;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IDropCommand))]
	public class DropCommand : EamonRT.Game.Commands.DropCommand, IDropCommand
	{
		protected override void PrintWearingRemoveFirst(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You're wearing {0}.  Remove {0} first.", artifact.EvalPlural("it", "them"));
		}
	}
}
