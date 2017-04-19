
// BeforePlayerMoveState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforePlayerMoveState : State, IBeforePlayerMoveState
	{
		public virtual Enums.Direction Direction { get; set; }

		protected virtual void ProcessEvents()
		{
			if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0 && Globals.GameState.Lt > 0)
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override void Execute()
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), Direction));

			var room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(room != null);

			Globals.GameState.R2 = room.GetDirs(Direction);

			ProcessEvents();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;
				});
			}

			Globals.NextState = NextState;
		}

		public BeforePlayerMoveState()
		{
			Name = "BeforePlayerMoveState";
		}
	}
}
