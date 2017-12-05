
// MainLoop.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IMainLoop))]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			base.Shutdown();

			// Duke Luxom's reward

			var monster = Globals.MDB[3];

			Debug.Assert(monster != null);

			if (monster.Location == Globals.GameState.Ro && monster.Friendliness > Enums.Friendliness.Enemy)
			{
				var reward = Globals.Character.GetStats(Enums.Stat.Charisma) * 7;

				Globals.Character.HeldGold += reward;

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Additionally, you receive {1} gold pieces for the safe return of Cynthia.{0}", Environment.NewLine, reward);

				Globals.In.KeyPress(Globals.Buf);
			}
		}
	}
}
