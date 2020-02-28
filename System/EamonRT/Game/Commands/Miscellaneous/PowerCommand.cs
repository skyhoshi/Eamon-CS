
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : Command, IPowerCommand
	{
		/// <summary>
		/// An event that fires after the player's spell cast attempt has resolved as successful.
		/// </summary>
		public const long PpeAfterPlayerSpellCastCheck = 1;

		public virtual bool CastSpell { get; set; }

		public virtual void PrintSonicBoom()
		{
			if (Globals.IsRulesetVersion(5))
			{
				gOut.Print("You hear a very loud sonic boom that echoes through the {0}.", gActorRoom.EvalRoomType("tunnels", "area"));
			}
			else
			{
				gOut.Print("You hear a loud sonic boom which echoes all around you!");
			}
		}

		public virtual void PrintFortuneCookie()
		{
			var rl = gEngine.RollDice(1, 100, 0);

			gOut.Print("A fortune cookie appears in mid-air and explodes!  The smoking paper left behind reads, \"{0}\"  How strange.",
				rl > 50 ?
				"THE SECTION OF TUNNEL YOU ARE IN COLLAPSES AND YOU DIE." :
				"YOU SUDDENLY FIND YOU CANNOT CARRY ALL OF THE ITEMS YOU ARE CARRYING, AND THEY ALL FALL TO THE GROUND.");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			var rl = gEngine.RollDice(1, 100, 0);

			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				if (Globals.IsRulesetVersion(5))
				{
					// Raise the dead / Make stuff vanish

					if (gEngine.ResurrectDeadBodies(gActorRoom) || gEngine.MakeArtifactsVanish(gActorRoom))
					{
						goto Cleanup;
					}

					// 10% chance of death trap

					if (rl < 11)
					{
						gOut.Print("The section of {0} collapses and you die.", gActorRoom.EvalRoomType("tunnel you are in", "ground you are on"));

						gGameState.Die = 1;

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
						gOut.Print("All of your wounds are healed.");

						gActorMonster.DmgTaken = 0;
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
			}

		Cleanup:

			;
		}

		public override void PlayerExecute()
		{
			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Power, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			PlayerProcessEvents(PpeAfterPlayerSpellCastCheck);

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

			Type = CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}
