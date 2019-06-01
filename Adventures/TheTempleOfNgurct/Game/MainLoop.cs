
// MainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			var reward = 0L;

			var medallionArtifact = Globals.ADB[77];

			Debug.Assert(medallionArtifact != null);

			var carryingMedallion = medallionArtifact.IsCarriedByCharacter();

			medallionArtifact.SetInLimbo();

			base.Shutdown();

			Globals.Out.Print("{0}", Globals.LineSep);

			// Thera's reward

			var theraMonster = Globals.MDB[29];

			Debug.Assert(theraMonster != null);

			var gonzalesMonster = Globals.MDB[55];

			Debug.Assert(gonzalesMonster != null);

			if (theraMonster.Location == Globals.GameState.Ro && theraMonster.Friendliness > Friendliness.Enemy)
			{
				var rw = 200 + (long)Math.Round(100 * ((double)(theraMonster.Hardiness - theraMonster.DmgTaken) / (double)theraMonster.Hardiness));

				Globals.Out.Print("In addition, you receive {0} gold piece{1} as a reward for the return of Princess Thera.{2}", 
					rw, 
					rw != 1 ? "s" : "", 
					gonzalesMonster.Location == Globals.GameState.Ro ? "  Of course, Gonzales takes his half." : "");

				if (gonzalesMonster.Location == Globals.GameState.Ro)
				{
					rw /= 2;
				}

				reward += rw;
			}

			// King's reward

			if (carryingMedallion)
			{
				Globals.Out.Print("You also receive the 2,000 gold pieces for the return of the gold medallion of Ngurct.  It is immediately destroyed by the King.");

				reward += 2000;
			}
			else
			{
				Globals.Out.Print("Unfortunately, you have failed in your mission to return the gold medallion of Ngurct.  Shame!  Shame!  (And no money.)");
			}

			Globals.Character.HeldGold += reward;

			Globals.In.KeyPress(Globals.Buf);
		}
	}
}
