
// MainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Startup()
		{
			RetCode rc;

			base.Startup();

			// Snapshot game state so it can be reverted when player wakes up

			rc = Globals.SaveDatabase(Constants.SnapshotFileName);

			Debug.Assert(Globals.Engine.IsSuccess(rc));
		}
	}
}
