
// SmileCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SmileCommand : Command, ISmileCommand
	{
		public override void PlayerExecute()
		{
			var monsters = gEngine.GetEmotingMonsterList(gActorRoom, gActorMonster);

			if (monsters.Count > 0)
			{
				foreach (var monster in monsters)
				{
					gEngine.MonsterEmotes(monster);
				}

				gOut.WriteLine();
			}
			else
			{
				gOut.Print("Okay.");
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public SmileCommand()
		{
			SortOrder = 310;

			Name = "SmileCommand";

			Verb = "smile";

			Type = CommandType.Interactive;
		}
	}
}
