
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

			if (CastSpell && !Globals.RtEngine.CheckPlayerSpellCast(Enums.Spell.Heal, true))
			{
				goto Cleanup;
			}

			var isCharMonster = DobjMonster.IsCharacterMonster();

			if (DobjMonster.DmgTaken > 0)
			{
				Globals.Buf.SetFormat("{0}{1}",
					Environment.NewLine,
					isCharMonster ? "Your" :
					DobjMonster.EvalPlural(DobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf01),
													DobjMonster.GetDecoratedName02(true, true, false, true, Globals.Buf02)));

				if (!DobjMonster.IsCharacterMonster())
				{
					Globals.Engine.GetPossessiveName(Globals.Buf);
				}

				Globals.Buf.AppendFormat("{1}{0}", Environment.NewLine, " health improves!");

				Globals.Out.Write("{0}", Globals.Buf);

				var rl = 0L;

				var rc = Globals.Engine.RollDice(1, 12, 0, ref rl);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

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
			if (CommandParser.CurrToken < CommandParser.Tokens.Length)
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
