
// TimeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class TimeCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITimeCommand
	{
		public override void PlayerExecute()
		{
			if (gActorRoom.IsGroundsRoom())
			{
				if (gGameState.Hour <= 4 || gGameState.Hour >= 23)
				{
					gOut.Print("Without the sun, who knows?");
				}
				else if (gGameState.Hour <= 11)
				{
					gOut.Print("A good estimate would be around {0} a.m.", gGameState.Hour);
				}
				else
				{
					gOut.Print("A good estimate would be around {0} p.m.", gGameState.Hour > 12 ? gGameState.Hour - 12 : 12);
				}
			}
			else if (gActorRoom.IsCryptRoom())
			{
				gOut.Print("There's no way of knowing, not underground!");
			}
			else
			{
				gOut.Print("You can't trust your sense of time in this strange place.");
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public TimeCommand()
		{
			SortOrder = 450;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "TimeCommand";

			Verb = "time";

			Type = CommandType.Miscellaneous;
		}
	}
}
