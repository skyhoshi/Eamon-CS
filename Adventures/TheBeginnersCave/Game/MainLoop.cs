
// MainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, EamonRT.Framework.IMainLoop
	{
		public override void Shutdown()
		{
			base.Shutdown();

			// Duke Luxom's reward

			var cynthiaMonster = Globals.MDB[3];

			Debug.Assert(cynthiaMonster != null);

			if (cynthiaMonster.Location == Globals.GameState.Ro && cynthiaMonster.Friendliness > Enums.Friendliness.Enemy)
			{
				var reward = Globals.Character.GetStats(Enums.Stat.Charisma) * 7;

				Globals.Character.HeldGold += reward;

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("Additionally, you receive {0} gold piece{1} for the safe return of Cynthia.", reward, reward != 1 ? "s" : "");

				Globals.In.KeyPress(Globals.Buf);
			}
		}
	}
}
