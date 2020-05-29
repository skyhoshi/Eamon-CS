
// WaitCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class WaitCommand : EamonRT.Game.Commands.Command, Framework.Commands.IWaitCommand
	{
		public virtual long Minutes { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(Minutes >= 0 && Minutes <= 55);

			gOut.Print("Time passes.");

			gGameState.Minute += Minutes;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			long i;

			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				gCommandParser.ParseName();

				// Wait up to 55 minutes in increments of 5

				if (long.TryParse(gCommandParser.ObjData.Name, out i) && i > 0)
				{
					Minutes = i;

					while (Minutes % 5 != 0)
					{
						Minutes++;
					}

					if (Minutes > 55)
					{
						Minutes = 55;
					}
				}
			}

			// Restrict WaitCommand while enemies are present

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				Minutes = 0;
			}
		}

		public WaitCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "WaitCommand";

			Verb = "wait";

			Type = CommandType.Miscellaneous;

			Minutes = 15;
		}
	}
}
