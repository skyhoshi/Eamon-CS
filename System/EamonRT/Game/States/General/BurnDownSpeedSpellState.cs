
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : State, IBurnDownSpeedSpellState
	{
		protected virtual void PrintSpeedSpellExpired()
		{
			Globals.Out.Write("{0}Your speed spell has{1} expired!{0}", Environment.NewLine, Globals.IsRulesetVersion(5) ? " just" : "");
		}

		public override void Execute()
		{
			if (Globals.GameState.Speed > 0 && ShouldPreTurnProcess())
			{
				Globals.GameState.Speed--;

				if (Globals.GameState.Speed <= 0)
				{
					var stat = Globals.Engine.GetStats(Enums.Stat.Agility);

					Debug.Assert(stat != null);

					var characterMonster = Globals.MDB[Globals.GameState.Cm];

					Debug.Assert(characterMonster != null);

					characterMonster.Agility /= 2;

					if (characterMonster.Agility < stat.MinValue)
					{
						characterMonster.Agility = stat.MinValue;
					}

					var rl = Globals.Engine.RollDice01(1, 100, 0);

					if (rl > 80 || !Globals.IsRulesetVersion(5))
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
