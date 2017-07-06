
// SpeedCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SpeedCommand : Command, ISpeedCommand
	{
		public virtual bool CastSpell { get; set; }

		protected virtual void PrintFeelNewAgility()
		{
			Globals.Out.WriteLine("{0}You can feel the new agility flowing through you!", Environment.NewLine);
		}

		protected override void PlayerExecute()
		{
			if (CastSpell && !Globals.RtEngine.CheckPlayerSpellCast(Enums.Spell.Speed, true))
			{
				goto Cleanup;
			}

			if (Globals.GameState.Speed <= 0)
			{
				ActorMonster.Agility *= 2;
			}

			var rl = 0L;

			var rc = Globals.Engine.RollDice(1, 10, 0, ref rl);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.GameState.Speed += (rl + 10);

			PrintFeelNewAgility();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public SpeedCommand()
		{
			SortOrder = 350;

			Name = "SpeedCommand";

			Verb = "speed";

			Type = Enums.CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
