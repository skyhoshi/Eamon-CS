
// SmileCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SmileCommand : Command, ISmileCommand
	{
		protected override void PlayerExecute()
		{
			var monsters = Globals.Engine.GetMonsterList(ActorRoom.IsLit, m => m.IsInRoom(ActorRoom) && m != ActorMonster);

			if (monsters.Count() > 0)
			{
				foreach (var monster in monsters)
				{
					monster.ResolveFriendlinessPct(Globals.Character);

					Globals.Engine.MonsterSmiles(monster);
				}

				Globals.Out.WriteLine();
			}
			else
			{
				Globals.Out.WriteLine("{0}Okay.", Environment.NewLine);
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

			Type = Enums.CommandType.Interactive;
		}
	}
}
