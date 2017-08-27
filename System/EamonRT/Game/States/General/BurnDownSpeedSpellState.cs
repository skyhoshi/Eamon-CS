
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : State, IBurnDownSpeedSpellState
	{
		protected virtual void PrintSpeedSpellExpired()
		{
			Globals.Out.Write("{0}Your speed spell has{1} expired!{0}", Environment.NewLine, Globals.IsClassicVersion(5) ? " just" : "");
		}

		public override void Execute()
		{
			if (Globals.GameState.Speed > 0 && ShouldPreTurnProcess())
			{
				Globals.GameState.Speed--;

				if (Globals.GameState.Speed <= 0)
				{
					Globals.MDB[Globals.GameState.Cm].Agility /= 2;

					var rl = Globals.Engine.RollDice01(1, 100, 0);

					if (!Globals.IsClassicVersion(5) || rl > 80)
					{
						PrintSpeedSpellExpired();
					}
				}
			}
			
			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IRegenerateSpellAbilitiesState>();
			}

			Globals.NextState = NextState;
		}

		public BurnDownSpeedSpellState()
		{
			Name = "BurnDownSpeedSpellState";
		}
	}
}
