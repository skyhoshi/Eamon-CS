
// PowerCommand.cs

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
	public class PowerCommand : Command, IPowerCommand
	{
		public virtual bool CastSpell { get; set; }

		protected virtual void PrintSonicBoom()
		{
			Globals.Out.Write("{0}You hear a loud sonic boom which echoes all around you!{0}", Environment.NewLine);
		}

		protected virtual void PrintFortuneCookie()
		{
			var rl = Globals.Engine.RollDice01(1, 100, 0);

			Globals.Out.Write("{0}A fortune cookie appears in mid-air and explodes!  The smoking paper left behind reads, \"{1}\"  How strange.{0}",
				Environment.NewLine,
				rl > 50 ?
				"THE SECTION OF TUNNEL YOU ARE IN COLLAPSES AND YOU DIE." :
				"YOU SUDDENLY FIND YOU CANNOT CARRY ALL OF THE ITEMS YOU ARE CARRYING, AND THEY ALL FALL TO THE GROUND.");
		}

		protected virtual void PlayerProcessEvents()
		{
			var rl = 0L;

			var rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (rl > 50)
			{
				// 50% chance of boom

				PrintSonicBoom();
			}
			else
			{
				// 50% chance of fortune cookie

				PrintFortuneCookie();
			}
		}

		protected override void PlayerExecute()
		{
			if (CastSpell && !Globals.RtEngine.CheckPlayerSpellCast(Enums.Spell.Power, true))
			{
				goto Cleanup;
			}

			PlayerProcessEvents();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public PowerCommand()
		{
			SortOrder = 360;

			Name = "PowerCommand";

			Verb = "power";

			Type = Enums.CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
