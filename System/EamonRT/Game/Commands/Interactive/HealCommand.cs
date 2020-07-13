
// HealCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class HealCommand : Command, IHealCommand
	{
		public virtual bool CastSpell { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(DobjMonster != null);

			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Heal, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			var isCharMonster = DobjMonster.IsCharacterMonster();

			if (DobjMonster.DmgTaken > 0)
			{
				if (Globals.IsRulesetVersion(5, 15))
				{
					Globals.Buf.SetFormat("{0}Some of {1}", 
						Environment.NewLine,
						isCharMonster ? "your" :
						DobjMonster.EvalPlural(DobjMonster.GetTheName(buf: Globals.Buf01),
														DobjMonster.GetArticleName(false, true, false, true, Globals.Buf02)));
				}
				else
				{
					Globals.Buf.SetFormat("{0}{1}",
						Environment.NewLine,
						isCharMonster ? "Your" :
						DobjMonster.EvalPlural(DobjMonster.GetTheName(true, buf: Globals.Buf01),
														DobjMonster.GetArticleName(true, true, false, true, Globals.Buf02)));
				}

				if (!isCharMonster)
				{
					gEngine.GetPossessiveName(Globals.Buf);
				}

				if (Globals.IsRulesetVersion(5, 15))
				{
					Globals.Buf.AppendFormat(" wounds seem to clear up.{0}", Environment.NewLine);
				}
				else
				{
					Globals.Buf.AppendFormat(" health improves!{0}", Environment.NewLine);
				}

				gOut.Write("{0}", Globals.Buf);

				var rl = gEngine.RollDice(1, Globals.IsRulesetVersion(5, 15) ? 10 : 12, 0);

				DobjMonster.DmgTaken -= rl;
			}

			if (DobjMonster.DmgTaken < 0)
			{
				DobjMonster.DmgTaken = 0;
			}

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				isCharMonster ? "You" : DobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
				isCharMonster ? "are" : "is");

			DobjMonster.AddHealthStatus(Globals.Buf);

			gOut.Write("{0}", Globals.Buf);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public HealCommand()
		{
			SortOrder = 290;

			Name = "HealCommand";

			Verb = "heal";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
