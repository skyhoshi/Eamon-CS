
// SpeedCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
			Globals.Out.Print("You can feel the new agility flowing through you!");
		}

		public override void PlayerExecute()
		{
			if (CastSpell && !Globals.Engine.CheckPlayerSpellCast(Enums.Spell.Speed, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			if (Globals.GameState.Speed <= 0)
			{
				ActorMonster.Agility *= 2;
			}

			var rl = Globals.IsRulesetVersion(5) ? Globals.Engine.RollDice01(1, 25, 9) : Globals.Engine.RollDice01(1, 10, 10);

			Globals.GameState.Speed += rl;

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
