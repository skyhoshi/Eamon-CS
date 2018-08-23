
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		/// <summary>
		/// This event fires after a check has been made to resolve the player's spell cast
		/// attempt, and it resolves as successful.
		/// </summary>
		public const long PpeAfterPlayerSpellCastCheck = 1;

		public virtual bool CastSpell { get; set; }

		public virtual void PrintSonicBoom()
		{
			if (Globals.IsRulesetVersion(5))
			{
				Globals.Out.Print("You hear a very loud sonic boom that echoes through the {0}.", ActorRoom.EvalRoomType("tunnels", "area"));
			}
			else
			{
				Globals.Out.Print("You hear a loud sonic boom which echoes all around you!");
			}
		}

		public virtual void PrintFortuneCookie()
		{
			var rl = Globals.Engine.RollDice01(1, 100, 0);

			Globals.Out.Print("A fortune cookie appears in mid-air and explodes!  The smoking paper left behind reads, \"{0}\"  How strange.",
				rl > 50 ?
				"THE SECTION OF TUNNEL YOU ARE IN COLLAPSES AND YOU DIE." :
				"YOU SUDDENLY FIND YOU CANNOT CARRY ALL OF THE ITEMS YOU ARE CARRYING, AND THEY ALL FALL TO THE GROUND.");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			var rl = Globals.Engine.RollDice01(1, 100, 0);

			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
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
						Globals.Out.Print("The section of {0} collapses and you die.", ActorRoom.EvalRoomType("tunnel you are in", "ground you are on"));

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
						Globals.Out.Print("All of your wounds are healed.");

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
			}

		Cleanup:

			;
		}

		public override void PlayerExecute()
		{
			if (CastSpell && !Globals.Engine.CheckPlayerSpellCast(Enums.Spell.Power, ShouldAllowSkillGains()))
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

			Type = Enums.CommandType.Miscellaneous;

			CastSpell = true;
		}
	}
}

/* EamonCsCodeTemplate

// PowerCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{

	}
}
EamonCsCodeTemplate */
