
// MainLoop.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using BeginnersForest.Framework;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IMainLoop))]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Startup()
		{
			base.Startup();

			// Entrance/exit gate rooms already seen

			var room = Globals.RDB[1];

			Debug.Assert(room != null);

			room.Seen = true;

			room = Globals.RDB[33];

			Debug.Assert(room != null);

			room.Seen = true;
		}
	}
}
