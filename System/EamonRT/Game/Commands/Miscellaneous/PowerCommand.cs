
// PowerCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
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
			if (Globals.IsRulesetVersion(5))
			{
				Globals.Out.Write("{0}You hear a very loud sonic boom that echoes through the {1}.{0}", Environment.NewLine, ActorRoom.EvalRoomType("tunnels", "area"));
			}
			else
			{
				Globals.Out.Write("{0}You hear a loud sonic boom which echoes all around you!{0}", Environment.NewLine);
			}
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
			var rl = Globals.Engine.RollDice01(1, 100, 0);

			if (Globals.IsRulesetVersion(5))
			{
				// Raise the dead / Make stuff vanish

				if (Globals.Engine.ResurrectDeadBodies() || Globals.Engine.MakeArtifactsVanish())
				{
					goto Cleanup;
				}

				// 10% chance of death trap

				if (rl < 11)
				{
					Globals.Out.Write("{0}The section of {1} collapses and you die.{0}", Environment.NewLine, ActorRoom.EvalRoomType("tunnel you are in", "ground you are on"));

					Globals.GameState.Die = 1;

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}

				// 75% chance of boom

				else if (rl < 86)
				{
					PrintSonicBoom();
				}

				// 5% chance of full heal

				else if (rl > 95)
				{
					Globals.Out.Write("{0}All of your wounds are healed.{0}", Environment.NewLine);

					ActorMonster.DmgTaken = 0;

					Globals.Engine.CheckEnemies();
				}

				// 10% chance of SPEED spell

				else
				{
					var command = Globals.CreateInstance<ISpeedCommand>(x =>
					{
						x.CastSpell = false;
					});

					CopyCommandData(command);

					NextState = command;
				}
			}
			else
			{
				// 50% chance of boom

				if (rl > 50)
				{
					PrintSonicBoom();
				}

				// 50% chance of fortune cookie

				else
				{
					PrintFortuneCookie();
				}
			}

		Cleanup:

			;
		}

		protected override void PlayerExecute()
		{
			if (CastSpell && !Globals.Engine.CheckPlayerSpellCast(Enums.Spell.Power, true))
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
