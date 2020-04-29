
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
			Debug.Assert(gDobjMonster != null);

			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Heal, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			var isCharMonster = gDobjMonster.IsCharacterMonster();

			if (gDobjMonster.DmgTaken > 0)
			{
				if (Globals.IsRulesetVersion(5, 15))
				{
					Globals.Buf.SetFormat("{0}Some of {1}", 
						Environment.NewLine,
						isCharMonster ? "your" :
						gDobjMonster.EvalPlural(gDobjMonster.GetTheName(buf: Globals.Buf01),
														gDobjMonster.GetArticleName(false, true, false, true, Globals.Buf02)));
				}
				else
				{
					Globals.Buf.SetFormat("{0}{1}",
						Environment.NewLine,
						isCharMonster ? "Your" :
						gDobjMonster.EvalPlural(gDobjMonster.GetTheName(true, buf: Globals.Buf01),
														gDobjMonster.GetArticleName(true, true, false, true, Globals.Buf02)));
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

				gDobjMonster.DmgTaken -= rl;
			}

			if (gDobjMonster.DmgTaken < 0)
			{
				gDobjMonster.DmgTaken = 0;
			}

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				isCharMonster ? "You" : gDobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
				isCharMonster ? "are" : "is");

			gDobjMonster.AddHealthStatus(Globals.Buf);

			gOut.Write("{0}", Globals.Buf);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (!Globals.IsRulesetVersion(5) && gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				PlayerResolveMonster();
			}
			else
			{
				Dobj = gMDB[gGameState.Cm];
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
