
// MainLoop.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IMainLoop))]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			var reward = 0L;

			var medallionArtifact = Globals.ADB[77];

			Debug.Assert(medallionArtifact != null);

			var carryingMedallion = medallionArtifact.IsCarriedByCharacter();

			Globals.Engine.RemoveWeight(medallionArtifact);

			medallionArtifact.SetInLimbo();

			base.Shutdown();

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			// Thera's reward

			var theraMonster = Globals.MDB[29];

			Debug.Assert(theraMonster != null);

			var gonzalesMonster = Globals.MDB[55];

			Debug.Assert(gonzalesMonster != null);

			if (theraMonster.Location == Globals.GameState.Ro && theraMonster.Friendliness > Enums.Friendliness.Enemy)
			{
				var rw = 200 + (long)Math.Round(100 * ((double)(theraMonster.Hardiness - theraMonster.DmgTaken) / (double)theraMonster.Hardiness));

				Globals.Out.Write("{0}In addition, you receive {1} gold pieces as a reward for the return of Princess Thera.{2}{0}", Environment.NewLine, rw, gonzalesMonster.Location == Globals.GameState.Ro ? "  Of course, Gonzales takes his half." : "");

				if (gonzalesMonster.Location == Globals.GameState.Ro)
				{
					rw /= 2;
				}

				reward += rw;
			}

			// King's reward

			if (carryingMedallion)
			{
				Globals.Out.Write("{0}You also receive the 2,000 gold pieces for the return of the gold medallion of Ngurct.  It is immediately destroyed by the King.{0}", Environment.NewLine);

				reward += 2000;
			}
			else
			{
				Globals.Out.Write("{0}Unfortunately, you have failed in your mission to return the gold medallion of Ngurct.  Shame!  Shame!  (And no money.){0}", Environment.NewLine);
			}

			Globals.Character.HeldGold += reward;

			if (Globals.Character.HeldGold < Constants.MinGoldValue)
			{
				Globals.Character.HeldGold = Constants.MinGoldValue;
			}
			else if (Globals.Character.HeldGold > Constants.MaxGoldValue)
			{
				Globals.Character.HeldGold = Constants.MaxGoldValue;
			}

			Globals.In.KeyPress(Globals.Buf);
		}
	}
}
