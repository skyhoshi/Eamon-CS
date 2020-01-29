
// SpeedCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SpeedCommand : Command, ISpeedCommand
	{
		public virtual bool CastSpell { get; set; }

		public virtual void PrintFeelNewAgility()
		{
			gOut.Print("You can feel the new agility flowing through you!");
		}

		public override void PlayerExecute()
		{
			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Speed, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			if (gGameState.Speed <= 0)
			{
				gActorMonster.Agility *= 2;
			}

			var rl = Globals.IsRulesetVersion(5) ? gEngine.RollDice(1, 25, 9) : gEngine.RollDice(1, 10, 10);

			gGameState.Speed += (rl + 1);

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

			Type = CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
