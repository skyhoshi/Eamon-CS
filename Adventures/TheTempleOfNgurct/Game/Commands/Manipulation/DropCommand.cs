
// DropCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
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

			Globals.Out.WriteLine("{0}You're wearing {1}.  Remove {1} first.", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}
	}
}
