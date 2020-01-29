
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : State, IBurnDownSpeedSpellState
	{
		public virtual void PrintSpeedSpellExpired()
		{
			gOut.Print("Your speed spell has{0} expired!", Globals.IsRulesetVersion(5) ? " just" : "");
		}

		public override void Execute()
		{
			if (gGameState.Speed > 0 && ShouldPreTurnProcess())
			{
				gGameState.Speed--;

				if (gGameState.Speed <= 0)
				{
					var stat = gEngine.GetStats(Stat.Agility);

					Debug.Assert(stat != null);

					var characterMonster = gMDB[gGameState.Cm];

					Debug.Assert(characterMonster != null);

					characterMonster.Agility /= 2;

					if (characterMonster.Agility < stat.MinValue)
					{
						characterMonster.Agility = stat.MinValue;
					}

					var rl = gEngine.RollDice(1, 100, 0);

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
