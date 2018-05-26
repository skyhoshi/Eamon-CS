
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : State, IEndOfRoundState
	{
		/// <summary>
		/// This event fires at the end of the current round, after all processing has been done.
		/// </summary>
		protected const long PeAfterRoundEnd = 1;

		public override void Execute()
		{
			Globals.Engine.CheckEnemies();

			var monsters = Globals.Database.MonsterTable.Records.ToList();

			foreach (var monster in monsters)
			{
				monster.InitGroupCount = monster.GroupCount;
			}

			ProcessEvents(PeAfterRoundEnd);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>(); 
			}

			Globals.NextState = NextState;
		}

		public EndOfRoundState()
		{
			Name = "EndOfRoundState";
		}
	}
}
