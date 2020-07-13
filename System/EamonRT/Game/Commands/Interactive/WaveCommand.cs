
// WaveCommand.cs

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
	public class WaveCommand : Command, IWaveCommand
	{
		public override void PlayerExecute()
		{
			var monsters = gEngine.GetEmotingMonsterList(ActorRoom, ActorMonster, false);

			if (monsters.Count > 0)
			{
				foreach (var monster in monsters)
				{
					gEngine.MonsterEmotes(monster, false);
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

		public WaveCommand()
		{
			SortOrder = 315;

			Name = "WaveCommand";

			Verb = "wave";

			Type = CommandType.Interactive;
		}
	}
}
