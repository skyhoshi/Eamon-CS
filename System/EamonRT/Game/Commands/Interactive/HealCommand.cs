
// HealCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class HealCommand : Command, IHealCommand
	{
		public virtual bool CastSpell { get; set; }

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjMonster != null);

			if (CastSpell && !Globals.Engine.CheckPlayerSpellCast(Enums.Spell.Heal, true))
			{
				goto Cleanup;
			}

			var isCharMonster = DobjMonster.IsCharacterMonster();

			if (DobjMonster.DmgTaken > 0)
			{
				if (Globals.IsRulesetVersion(5))
				{
					Globals.Buf.SetFormat("{0}Some of your", Environment.NewLine);
				}
				else
				{
					Globals.Buf.SetFormat("{0}{1}",
						Environment.NewLine,
						isCharMonster ? "Your" :
						DobjMonster.EvalPlural(DobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf01),
														DobjMonster.GetDecoratedName02(true, true, false, true, Globals.Buf02)));
				}

				if (!isCharMonster)
				{
					Globals.Engine.GetPossessiveName(Globals.Buf);
				}

				if (Globals.IsRulesetVersion(5))
				{
					Globals.Buf.AppendFormat(" wounds seem to clear up.{0}", Environment.NewLine);
				}
				else
				{
					Globals.Buf.AppendFormat(" health improves!{0}", Environment.NewLine);
				}

				Globals.Out.Write("{0}", Globals.Buf);

				var rl = Globals.Engine.RollDice01(1, Globals.IsRulesetVersion(5) ? 10 : 12, 0);

				if (Globals.IsRulesetVersion(5))
				{
					Globals.GameState.ModDTTL(DobjMonster.Friendliness, -Math.Min(DobjMonster.DmgTaken, rl));
				}

				DobjMonster.DmgTaken -= rl;
			}

			if (DobjMonster.DmgTaken < 0)
			{
				DobjMonster.DmgTaken = 0;
			}

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				isCharMonster ? "You" : DobjMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01),
				isCharMonster ? "are" : "is");

			DobjMonster.AddHealthStatus(Globals.Buf);

			Globals.Out.Write("{0}", Globals.Buf);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			if (!Globals.IsRulesetVersion(5) && CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				PlayerResolveMonster();
			}
			else
			{
				DobjMonster = Globals.MDB[Globals.GameState.Cm];
			}
		}

		public HealCommand()
		{
			SortOrder = 290;

			Name = "HealCommand";

			Verb = "heal";

			Type = Enums.CommandType.Interactive;

			CastSpell = true;
		}
	}
}
