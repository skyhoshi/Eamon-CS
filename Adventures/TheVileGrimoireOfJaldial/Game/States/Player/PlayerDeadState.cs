
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PlayerDeadState : EamonRT.Game.States.PlayerDeadState, IPlayerDeadState
	{
		public override void Execute()
		{
			if (gGameState.PlayerResurrections <= 2)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

				Globals.Buf.Clear();

				var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				gOut.Print("{0}", Globals.LineSep);

				if (++gGameState.PlayerResurrections <= 2)
				{
					NextState = Globals.CreateInstance<Framework.States.IPlayerResurrectedState>();

					Globals.NextState = NextState;
				}
				else
				{
					gOut.Print("You feel yourself spinning into infinity, and then re-embodied in your former self.  As you slowly open your eyes, with a sense of bewilderment, you find yourself once again in the open grave.");

					gOut.Print("However, as you look upward, you see the hideous figure of a gargoyle appear at the grave's edge.  Moments later, a ghoul appears, clutching a shovel.  They glance at each other, then peer down at you with evil contempt.");

					gOut.Print("You hear the gargoyle hiss, and with that, the ghoul begins to labor.  A shower of dirt hits your face, but in your current dazed, defenseless state all you can do is splutter, gasping for air.");

					gOut.Print("It all ends for you here, buried alive in your custom-made grave.");
				}
			}

			if (gGameState.PlayerResurrections > 2)
			{
				base.Execute();
			}
		}
	}
}
