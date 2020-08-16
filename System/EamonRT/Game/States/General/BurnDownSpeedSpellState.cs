﻿
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : State, IBurnDownSpeedSpellState
	{
		/// <summary></summary>
		public virtual IStat AgilityStat { get; set; }

		/// <summary></summary>
		public virtual long PrintSpellExpiredRoll { get; set; }

		public override void Execute()
		{
			if (gGameState.Speed > 0 && (!Globals.CommandPromptSeen || ShouldPreTurnProcess()))
			{
				gGameState.Speed--;

				if (gGameState.Speed <= 0)
				{
					AgilityStat = gEngine.GetStats(Eamon.Framework.Primitive.Enums.Stat.Agility);

					Debug.Assert(AgilityStat != null);

					Debug.Assert(gCharMonster != null);

					gCharMonster.Agility /= 2;

					if (gCharMonster.Agility < AgilityStat.MinValue)
					{
						gCharMonster.Agility = AgilityStat.MinValue;
					}

					PrintSpellExpiredRoll = gEngine.RollDice(1, 100, 0);

					if (PrintSpellExpiredRoll > 80 || !Globals.IsRulesetVersion(5, 15))
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

		public virtual void PrintSpeedSpellExpired()
		{
			gOut.Print("Your speed spell has{0} expired!", Globals.IsRulesetVersion(5, 15) ? " just" : "");
		}

		public BurnDownSpeedSpellState()
		{
			Name = "BurnDownSpeedSpellState";
		}
	}
}
